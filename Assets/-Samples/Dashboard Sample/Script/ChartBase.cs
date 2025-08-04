using E2C;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static E2C.E2ChartData;

namespace Sample.Dashboard
{
    /// <summary>
    /// E2Chart를 활용한 Chart Controller의 Base 함수. <br/>
    /// 더 많은 차트 형식은 Assets/Ice pond/E2Chart/Examples/Scenes/Demo.unity를 참조.
    /// </summary>
    public abstract class ChartBase
    {
        protected E2Chart chart;
        protected Series series;
        protected E2ChartOptions chartOption;

        protected ChartBase(E2Chart chart)
        {
            this.chart = chart;
        }

        public void UpdateChart()
        {
            chart.UpdateChart();
        }

        public void ClearChart()
        {
            ClearData();
            ClearCategory();
        }

        /// <summary>
        /// 차트를 초기화한다.
        /// </summary>
        protected abstract void InitChart();

        /// <summary>
        /// 표현할 차트 형식에 따라 옵션을 초기화한다. 인스펙터에서 설정해도 된다. <br/>
        /// ex) chartOption.yAxis.enableLabel = true;
        /// </summary>
        protected abstract void InitChartOption();

        /// <summary>
        /// 표현할 차트 형식에 따라 series.dataX와 series.dataY 중 하나를 선택하여 사용한다.
        /// </summary>
        public abstract void AddData(List<float> dataList);

        /// <summary>
        /// 표현할 차트 형식에 따라 ClearDataY와 ClearDataX 중 하나를 선택하여 사용한다. <br/>
        /// ex) 세로 차트일 경우 ClearDataY를, 반대의 경우는 ClearDataX를 사용한다.
        /// </summary>
        public abstract void ClearData();

        /// <summary>
        /// 표현할 차트 형식에 따라 categoriesX와 categoriesY 중 하나를 선택하여 사용한다. <br/>
        /// </summary>
        public abstract void AddCategory();

        /// <summary>
        /// 표현할 차트 형식에 따라 ClearCategoryX와 ClearCategoryY 중 하나를 선택하여 사용한다. <br/>
        /// ex) 세로 차트일 경우 ClearCategoryX를, 반대의 경우는 ClearCategoryY를 사용한다.
        /// </summary>
        public abstract void ClearCategory();

        /// <summary>
        /// 표현할 차트 형식에 따라 수정하여 사용한다. <br/>
        /// ex) 세로 차트일 경우 chartOption.yAxis를, 반대의 경우는 chartOption.xAxis를 조정한다.
        /// </summary>
        protected abstract void SetChartAxisInterval(float maxValue);

        /// <summary>
        /// 표현할 차트의 형식을 지정한다.
        /// </summary>
        protected void SetChartType(E2Chart.ChartType type)
        {
            chart.chartType = type;
        }

        /// <summary>
        /// 차트의 Series를 모두 지운다.
        /// </summary>
        protected void ClearSieres()
        {
            if (chart.chartData.series != null)
            {
                chart.chartData.series.Clear();
            }
        }

        /// <summary>
        /// 해당 Series의 X축 Data를 지운다.
        /// </summary>
        protected void ClearDataX(Series series)
        {
            if (series.dataX != null)
            {
                series.dataX.Clear();
                series.dataX = null;
            }
        }

        /// <summary>
        /// 해당 Series의 Y축 Data를 지운다.
        /// </summary>
        protected void ClearDataY(Series series)
        {
            if (series.dataY != null)
            {
                series.dataY.Clear();
                series.dataY = null;
            }
        }

        /// <summary>
        /// 차트의 X축 Category를 지운다.
        /// </summary>
        protected void ClearCategoryX()
        {
            if (chart != null && chart.chartData != null)
            {
                chart.chartData.categoriesX.Clear();
                chart.chartData.categoriesX = null;

                chart.chartData.categoriesX = new List<string>();
            }
        }

        /// <summary>
        /// 차트의 Y축 Category를 지운다.
        /// </summary>
        protected void ClearCategoryY()
        {
            if (chart != null && chart.chartData != null)
            {
                chart.chartData.categoriesY.Clear();
                chart.chartData.categoriesY = null;

                chart.chartData.categoriesY = new List<string>();
            }
        }

        /// <summary>
        /// 해당 데이터의 Max값을 리턴한다.
        /// </summary>
        protected float GetMaxValue(List<float> dataList)
        {
            float maxValue = 0.0f;

            foreach (var each in dataList)
            {
                if (maxValue < each)
                {
                    maxValue = each;
                }
            }

            return maxValue;
        }

        /// <summary>
        /// 해당 인덱스의 차트 색상을 변경한다.
        /// </summary>
        protected void SetPlotColor(int index, Color color)
        {
            chart.chartOptions.plotOptions.seriesColors[index] = color;
        }
    }
}