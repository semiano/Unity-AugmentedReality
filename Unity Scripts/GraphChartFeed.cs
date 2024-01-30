#define Graph_And_Chart_PRO
using UnityEngine;
using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;

public class GraphChartFeed : MonoBehaviour
{
    private List<float> datapoints1 = new List<float>();
    private List<float> datapoints2 = new List<float>();
    public int maxPoints;
    float elapsed = 0f;

    void Start ()
    {
       
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            RenderGraph(); //FUNCTION CALL
        }
    }

    float getNewDataPoint()
    {
        float val = Random.value * 10f + 20f;
        return val;
    }

    void RenderGraph()
    {
        GraphChartBase graph = GetComponent<GraphChartBase>();
        if (graph != null)
        {
            graph.HorizontalValueToStringMap[0.0] = "Zero"; // example of how to set custom axis strings
            graph.DataSource.StartBatch();
            graph.DataSource.ClearCategory("Series 1"); //straight graph
            graph.DataSource.ClearAndMakeBezierCurve("Series 1"); //curved graph only
            graph.HorizontalPanning = true;
            graph.DataSource.HorizontalViewSize =maxPoints;
            graph.DataSource.VerticalViewSize = 77.0;

            if (datapoints1.Count < maxPoints)
            {
                datapoints1.Add(getNewDataPoint());
            }
            else
            {
                datapoints1.RemoveAt(0);
                datapoints1.Add(getNewDataPoint());
            }

            for (int i = 0; i < datapoints1.Count; i++)
            {
                if (i == 0)
                {
                    graph.DataSource.SetCurveInitialPoint("Series 1", i, datapoints1[i]); //curved graph only
                }
                //graph.DataSource.AddPointToCategory("Series 1", i, datapoints1[i]); //straight graph
                graph.DataSource.AddLinearCurveToCategory("Series 1", new DoubleVector2(i, datapoints1[i])); //curved graph only
                graph.DataSource.MakeCurveCategorySmooth("Series 1"); //curved graph only
            }

            //for (int i = 0; i < 30; i++)
            //    {
            //        graph.DataSource.AddPointToCategory("Series 1", i, Random.value * 10f + 20f);
            //        if (i == 0)
            //            graph.DataSource.SetCurveInitialPoint("Player 2", i, Random.value * 10f + 10f);
            //        else
            //            graph.DataSource.AddLinearCurveToCategory("Player 2",
            //                                                            new DoubleVector2(i, Random.value * 10f + 10f));
            //    }
            graph.DataSource.EndBatch();
            }
    }

}
