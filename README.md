# WWWNetworking

I often found myself writing the same code over and over when dealing with [Unity](http://unity3d.com/)'s `WWW` class. This library provides easy use of many of those patterns.

## `NetworkEngine`
This is the primary interface to the library. It is also available as a singleton with the class `NetworkEngineSingleton`. Basic example:

    using UnityEngine;
    using WWWNetworking;
    public class FloatingImage : MonoBehaviour {
      void Start() {
        NetworkEngineSingleton.Instance.Download("http://images.earthcam.com/ec_metros/ourcams/fridays.jpg", www => {
          GetComponent<Renderer>().material.mainTexture = www.texture;
        });
      }
    }
Each instance has a configurable maximum number of concurrent requests. Increasing the maximum immediately starts running more requests if queued while decreasing leaves requests running if over the maximum.

The network engine can technically run anything which is an `IRequest`. The requests don't necessarily need to be network requests. You can easily implement a request which does something completely different.

## `Request` Classes
These are pre-built classes for common tasks with downloading files and running some code using that file. I will add to the included ones over time. Feel free to submit a pull-request for a new one. You should extract any data from the `WWW` object in the success callback since it is disposed afterward.

### Request
A basic request wich downloads a file and runs code on success.

### RequestWithError
Downloads a file and provides callbacks for both success and error. All remaining request classes inherit from this.

### ProgressRequest
Downloads a file and provides callbacks for progress updates, successful completion, and error.

### FormRequest
Performs a post to a url with form data. Provides callbacks for success and error.

### FormProgressRequest
Performs a post to a url with form data. Provides callbacks for upload progress, download progress, successful completion, and error.

### CachableAssetBundleRequest
Loads an asset bundle from the cache or downloads when not in the cache.

## Appendix
For anyone curious, the URL in the examples is the same one used in Unity's official documentation for [WWW.texture](http://docs.unity3d.com/ScriptReference/WWW-texture.html).
