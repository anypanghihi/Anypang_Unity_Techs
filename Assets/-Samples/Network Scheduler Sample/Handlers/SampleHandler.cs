using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

#region "Handlers"
public class SampleHandler : INetworkResponseHandler<NetworkResult>
{
    public bool CanHandle()
    {
        return true;
    }

    public void HandleResponse(NetworkResult result)
    {
        Debug.Log("[SampleHandler] HandleResponse 호출됨");

        Debug.Log(result.Response);

        try
        {
            var response = JsonConvert.DeserializeObject<SlideshowResponse>(result.Response);

            if (response?.slideshow == null)
            {
                Debug.LogWarning("[SampleHandler] slideshow가 null입니다.");
                return;
            }

            Debug.Log($"[SampleHandler] 결과: {response.slideshow.author}");

            if (response.slideshow.slides != null && response.slideshow.slides.Count > 0)
            {
                foreach (var slide in response.slideshow.slides)
                {
                    Debug.Log($"[SampleHandler] 슬라이드 제목: {slide.title}");
                    Debug.Log($"[SampleHandler] 슬라이드 타입: {slide.type}");

                    if (slide.items != null)
                    {
                        foreach (var item in slide.items)
                        {
                            Debug.Log($"[SampleHandler] 아이템: {item}");
                        }
                    }
                    else
                    {
                        Debug.Log("[SampleHandler] 아이템이 없습니다.");
                    }
                }
            }
            else
            {
                Debug.Log("[SampleHandler] 슬라이드가 없습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SampleHandler] 파싱 오류: {ex.Message}");
        }
    }
}
#endregion

/// <summary>
/// 여기에다 파싱 형식 구현
/// System.Text.Json을 통한 Deserize는 Property로 설정하여야 함.
/// </summary>
#region "Models"
[Serializable]
public class SlideshowResponse
{
    public Slideshow slideshow;
}

[Serializable]
public class Slideshow
{
    public string author;
    public string date;
    public List<Slide> slides;
    public string title;
}

[Serializable]
public class Slide
{
    public string title;
    public string type;
    public List<string> items;
}
#endregion