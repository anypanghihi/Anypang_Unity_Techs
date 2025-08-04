using UniRx;

namespace Sample.UniRX
{
    public class SampleViewModel
    {
        private MockSampleService _mockService;
        public ReactiveProperty<SampleData> SampleInfo { get; private set; }

        public SampleViewModel(MockSampleService mockService)
        {
            _mockService = mockService;
            SampleInfo = new ReactiveProperty<SampleData>();
        }

        public void LoadMockData()
        {
            SampleInfo.Value = _mockService.GetMockData();
        }
    }
}