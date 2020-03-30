using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
#if !UniRxLibrary
using ObservableUnity = UniRx.Observable;
#endif

namespace UniRx
{
#if !(UNITY_METRO || UNITY_WP8) && (UNITY_4_4 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
    // Fallback for Unity versions below 4.5
    using Hash = System.Collections.Hashtable;
    using HashEntry = System.Collections.DictionaryEntry;    
#else
    // Unity 4.5 release notes: 
    // WWW: deprecated 'WWW(string url, byte[] postData, Hashtable headers)', 
    // use 'public WWW(string url, byte[] postData, Dictionary<string, string> headers)' instead.
    using Hash = System.Collections.Generic.Dictionary<string, string>;
    using HashEntry = System.Collections.Generic.KeyValuePair<string, string>;
#endif

    public static partial class ObservableWWW
    {
        public delegate IEnumerator FetchHandler<T> (string url, Func<object, T> processingData, IObserver<T> observer,
            IProgress<float> reportProgress, CancellationToken cancel);
        
        public static IEnumerator Fetch<T>(string url, Func<object, T> processingData, IObserver<T> observer, 
            IProgress<float> reportProgress, CancellationToken cancel)
        {
            using (var www = new WWW(url, null, new Hash()))
            {
                if (reportProgress != null)
                {
                    while (!www.isDone && !cancel.IsCancellationRequested)
                    {
                        try
                        {
                            reportProgress.Report(www.progress);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            yield break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    if (!www.isDone)
                    {
                        yield return www;
                    }
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (reportProgress != null)
                {
                    try
                    {
                        reportProgress.Report(www.progress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    var headers = "Empty"; 
                    if (www.responseHeaders != null)
                    {
                        headers = string.Join(",", www.responseHeaders.Select(kvp => kvp.Key + "=" + kvp.Value).ToArray());
                    }
                    var error = string.Format("Can't load data from url {0}.\nError: {1}\n Bytes downloaded: {2}, " +
                                              "Text: '{3}', Headers: '{4}'"
                        , www.url, www.error, www.bytesDownloaded, www.text, headers);
                    observer.OnError(new NetworkErrorException(www, error));
                }
                else if (www.bytesDownloaded == 0)
                {
                    var error = string.Format("Can't load data from url {0}.\nError: Zero bytes were downloaded", www.url);
                    observer.OnError(new NetworkErrorException(www, error));
                }
                else
                {
                    var result = default(T);
                    Exception exception = null;

                    try
                    {
                        result = processingData(www);
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }
                    finally
                    {
                        if (exception != null)
                        {
                            observer.OnError(exception);
                        }
                        else
                        {
                            observer.OnNext(result);
                            observer.OnCompleted();
                        }
                    }
                }
            }
        }
     
        public static IEnumerator FetchAudio<T>(string url, Func<object, T> processingData, IObserver<T> observer, 
            IProgress<float> reportProgress, CancellationToken cancel)
        {
#if UNITY_EDITOR && UNITY_WEBGL           
            yield return new WaitForEndOfFrame();
            
            // We need to report that downloading is completed
            if (reportProgress != null)
            {
                reportProgress.Report(1f);
            }
            
            // Notify observer with default value
            observer.OnError(new Exception("Download and initiate sound from Editor/WebGL"));
#else
            using (var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                var ao = www.Send();

                if (reportProgress != null)
                {
                    while (!ao.isDone && !cancel.IsCancellationRequested)
                    {
                        try
                        {
                            reportProgress.Report(ao.progress);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            yield break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    if (!ao.isDone)
                    {
                        yield return ao;
                    }
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (reportProgress != null)
                {
                    try
                    {
                        reportProgress.Report(ao.progress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }
                }

                if (www.isNetworkError || www.isHttpError)
                {
                    var responceCodeField = www.responseCode +
                                            (www.responseCode == 0
                                                ? " (can be cross-origin problems)"
                                                : "");
                    observer.OnError(new Exception(string.Format(
                        "[SoundLoader] GetAudioClip error\nError : {0}\nClip url: {1}\nIsNetworkError? {2}\nIsHTTPError? {3}\nResponse code: {4}",
                        www.error,
                        url,
                        www.isNetworkError,
                        www.isHttpError,
                        responceCodeField)));
                }
                else
                {
                    var result = default(T);
                    var audioClip = DownloadHandlerAudioClip.GetContent(www);
                    
                    if (audioClip != null)
                    {
                        while (audioClip.loadState != AudioDataLoadState.Loaded &&
                               !cancel.IsCancellationRequested)
                        {
                            if (audioClip.loadState == AudioDataLoadState.Failed)
                            {
                                observer.OnError(new Exception("Fail to load audio clip" + audioClip.loadState));
                                yield break;
                            }
                            yield return null;
                        }

                        if (cancel.IsCancellationRequested)
                        {
                            yield break;
                        }
                        
                        result = (T) Convert.ChangeType(audioClip, typeof(T));
                    }

                    observer.OnNext(result);
                    observer.OnCompleted();
                }
            }
#endif
        }

        public static IObservable<T> ToSingleSubscribe<T>(this IObservable<T> source, int retryCount, TimeSpan delay, IScheduler scheduler)
        {
            var subj = new Subject<T>();
            source.OnErrorRetry((Exception e) => { }, retryCount, delay, scheduler).Subscribe(subj);
            return subj;
        }
    }
}