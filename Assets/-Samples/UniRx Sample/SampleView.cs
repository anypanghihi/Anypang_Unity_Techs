using UnityEngine;
using TMPro;
using UniRx;

namespace Sample.UniRX
{
    public class SampleView : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI valueText;
        public TextMeshProUGUI altValueText;
        private SampleViewModel _viewModel;

        private void Start()
        {
            var mockService = new MockSampleService();
            _viewModel = new SampleViewModel(mockService);

            // UniRx를 사용하여 ViewModel의 데이터 변경 시 자동 UI 업데이트
            _viewModel.SampleInfo.Subscribe(UpdateUI).AddTo(this);

            // Mock 데이터 로드
            _viewModel.LoadMockData();
        }

        private void UpdateUI(SampleData data)
        {
            if (data != null)
            {
                nameText.text = data.Name;
                valueText.text = data.Value.ToString();
                altValueText.text = data.AlternativeValue.ToString();
            }
        }
    }
}
