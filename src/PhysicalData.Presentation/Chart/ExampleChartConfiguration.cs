namespace PhysicalData.Presentation.Chart
{
    public static class ExampleChartConfiguration
    {
        public static object Data = new
        {
            Labels = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" },
            Datasets = new[]
                {
                    new
                    {
                        Label = "India",
                        Data = new List<double>{ 9, 20, 29, 33, 50, 66, 75, 86, 91, 105, 120, 126, 141, 150, 156, 164, 177, 180, 184, 195 },
                        BackgroundColor = new List<string>{ "rgb(88, 80, 141)" },
                        BorderColor = new List<string>{ "rgb(88, 80, 141)" },
                        BorderWidth = new List<double>{2},
						// HoverBorderWidth = new List<double>{4},
						PointBackgroundColor = new List<string>{ "rgb(88, 80, 141)" },
                        PointBorderColor = new List<string>{ "rgb(88, 80, 141)" },
                        PointRadius = new List<int>{0}, // hide points
                        PointHoverRadius = new List<int>{4},
                        yAxisID = "y0",
                        pointHitRadius = 20,
                    },
                    new
                    {
                        Label = "England",
                        Data = new List<double>{ 1, 1, 8, 19, 24, 26, 39, 47, 56, 66, 75, 88, 95, 100, 109, 114, 124, 129, 140, 142 },
                        BackgroundColor = new List<string>{ "rgb(255, 166, 0)" },
                        BorderColor = new List<string>{ "rgb(255, 166, 0)" },
                        BorderWidth = new List<double>{2},
						// HoverBorderWidth = new List<double>{4},
						PointBackgroundColor = new List<string>{ "rgb(255, 166, 0)" },
                        PointBorderColor = new List<string>{ "rgb(255, 166, 0)" },
                        PointRadius = new List<int>{0}, // hide points
                        PointHoverRadius = new List<int>{4},
                        yAxisID = "y0",
                        pointHitRadius = 20,
                    }
                }
        };

        public static object Option = new
        {
            responsive = true,
            maintainAspectRatio = true,
            interaction = new
            {
                mode = "point",
                axis = "xy",
                intersect = false
            },
            stacked = false,
            plugins = new
            {
                title = new
                {
                    display = true,
                    text = "Example line chart"
                },
                tooltip = new
                {
                    position = "nearest",
                },
                legend = new
                {
                    display = true,
                    position = "right"
                },
                tooltipLine = new
                {
                    display = true,
                }
            },
            scales = new
            {
                y0 = new
                {
                    type = "linear",
                    display = true,
                    position = "left"
                },
                //y1 = new
                //{
                //	type = "linear",
                //	display = true,
                //	position = "right",
                //	grid = new
                //	{
                //		drawOnChartArea = false
                //	},
                //	ticks = new
                //	{
                //		format = new
                //		{
                //			style = "percent",
                //			minimumFractionDigits = "0",
                //			maximumFractionDigits = "0"
                //		}
                //	}
                //},
            }
        };

        public static object ParseDataToDataset<T>(T[] sData, string sLabel, string sOrdinateId)
        {
            return new
            {
                Label = sLabel,
                Data = sData,
                BackgroundColor = new List<string> { "rgb(88, 80, 141)" },
                BorderColor = new List<string> { "rgb(88, 80, 141)" },
                YAxisID = sOrdinateId,
                Type = "line",
                Order = "0",
                BorderWidth = 2,
                PointRadius = 2,
            };
        }
    }
}