using E2C;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static E2C.E2ChartData;

namespace Sample.Dashboard
{
    /// <summary>
    /// E2Chart�� Ȱ���� Chart Controller�� Base �Լ�. <br/>
    /// �� ���� ��Ʈ ������ Assets/Ice pond/E2Chart/Examples/Scenes/Demo.unity�� ����.
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
        /// ��Ʈ�� �ʱ�ȭ�Ѵ�.
        /// </summary>
        protected abstract void InitChart();

        /// <summary>
        /// ǥ���� ��Ʈ ���Ŀ� ���� �ɼ��� �ʱ�ȭ�Ѵ�. �ν����Ϳ��� �����ص� �ȴ�. <br/>
        /// ex) chartOption.yAxis.enableLabel = true;
        /// </summary>
        protected abstract void InitChartOption();

        /// <summary>
        /// ǥ���� ��Ʈ ���Ŀ� ���� series.dataX�� series.dataY �� �ϳ��� �����Ͽ� ����Ѵ�.
        /// </summary>
        public abstract void AddData(List<float> dataList);

        /// <summary>
        /// ǥ���� ��Ʈ ���Ŀ� ���� ClearDataY�� ClearDataX �� �ϳ��� �����Ͽ� ����Ѵ�. <br/>
        /// ex) ���� ��Ʈ�� ��� ClearDataY��, �ݴ��� ���� ClearDataX�� ����Ѵ�.
        /// </summary>
        public abstract void ClearData();

        /// <summary>
        /// ǥ���� ��Ʈ ���Ŀ� ���� categoriesX�� categoriesY �� �ϳ��� �����Ͽ� ����Ѵ�. <br/>
        /// </summary>
        public abstract void AddCategory();

        /// <summary>
        /// ǥ���� ��Ʈ ���Ŀ� ���� ClearCategoryX�� ClearCategoryY �� �ϳ��� �����Ͽ� ����Ѵ�. <br/>
        /// ex) ���� ��Ʈ�� ��� ClearCategoryX��, �ݴ��� ���� ClearCategoryY�� ����Ѵ�.
        /// </summary>
        public abstract void ClearCategory();

        /// <summary>
        /// ǥ���� ��Ʈ ���Ŀ� ���� �����Ͽ� ����Ѵ�. <br/>
        /// ex) ���� ��Ʈ�� ��� chartOption.yAxis��, �ݴ��� ���� chartOption.xAxis�� �����Ѵ�.
        /// </summary>
        protected abstract void SetChartAxisInterval(float maxValue);

        /// <summary>
        /// ǥ���� ��Ʈ�� ������ �����Ѵ�.
        /// </summary>
        protected void SetChartType(E2Chart.ChartType type)
        {
            chart.chartType = type;
        }

        /// <summary>
        /// ��Ʈ�� Series�� ��� �����.
        /// </summary>
        protected void ClearSieres()
        {
            if (chart.chartData.series != null)
            {
                chart.chartData.series.Clear();
            }
        }

        /// <summary>
        /// �ش� Series�� X�� Data�� �����.
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
        /// �ش� Series�� Y�� Data�� �����.
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
        /// ��Ʈ�� X�� Category�� �����.
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
        /// ��Ʈ�� Y�� Category�� �����.
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
        /// �ش� �������� Max���� �����Ѵ�.
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
        /// �ش� �ε����� ��Ʈ ������ �����Ѵ�.
        /// </summary>
        protected void SetPlotColor(int index, Color color)
        {
            chart.chartOptions.plotOptions.seriesColors[index] = color;
        }
    }
}