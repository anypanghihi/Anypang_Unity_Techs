using E2C;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sample.Dashboard
{
    public class SampleChartController : ChartBase
    {
        public SampleChartController(E2Chart chart) : base(chart)
        {
            InitChart();
        }

        protected override void InitChart()
        {
            ClearSieres();
            ClearCategoryX();
            ClearCategoryY();

            SetChartType(E2Chart.ChartType.BarChart);

            series = new E2ChartData.Series();
            chart.chartData.series.Add(series);

            chartOption = chart.chartOptions;

            InitChartOption();
        }

        protected override void InitChartOption()
        {

        }

        public override void AddCategory()
        {
            for (int i = 0; i < 12; i++)
            {
                chart.chartData.categoriesX.Add(i.ToString());
            }
        }

        public override void AddData(List<float> dataList)
        {
            float maxValue = GetMaxValue(dataList);
            SetChartAxisInterval(maxValue);

            series.dataY = dataList;
        }

        public override void ClearCategory()
        {
            ClearCategoryX();
        }

        public override void ClearData()
        {
            ClearDataY(series);
        }

        protected override void SetChartAxisInterval(float maxValue)
        {
            if (maxValue == 0)
            {
                chartOption.yAxis.max = 200.0f;
                chartOption.yAxis.axisDivision = 5;
            }
            else
            {
                int maxCount = (int)maxValue;
                chartOption.yAxis.max = maxCount + 1;
                chartOption.yAxis.axisDivision = (maxCount > 5) ? 5 : 1;
            }
        }
    }
}
