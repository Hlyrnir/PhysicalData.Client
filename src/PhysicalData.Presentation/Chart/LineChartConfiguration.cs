namespace PhysicalData.Presentation.Chart
{
    public static class LineChartConfiguration
    {
        private static string sLineFillColor = "rgb(88, 80, 141)";
        private static string sAnotherLineFillColor = "rgb(88, 80, 141)";

        public static object Option = new
        {
            responsive = false,
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
                    position = "nearest"
                },
                legend = new
                {
                    display = true,
                    position = "right"
                },
                tooltipLine = new
                {
                    display = true,
                    index = 2
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
            },
        };

        public static object Data = new
        {
            labels = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" },
            datasets = new[]
            {
                new
                {
                    label = "data_00",
                    data = new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" },
                    backgroundColor = new[] { sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor },
                    borderColor = sAnotherLineFillColor,
                    yAxisID = "y0",
                    type = "line",
                    order = "0",
                    borderWidth = 2,
                    pointRadius = 2,
                },
                new
                {
                    label = "data_01",
                    data = new[] { "0", "1", "4", "9", "16", "25", "36", "49", "64", "81" },
                    backgroundColor = new[] { sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor },
                    borderColor = sLineFillColor,
                    yAxisID = "y0",
                    type = "line",
                    order = "0",
                    borderWidth = 2,
                    pointRadius = 2,
                },
                new
                {
                    label = "data_02",
                    data = new[] { "0.172", "0.172", "0.271", "0.359", "0.435", "0.571", "0.781", "0.90", "1.0", "1.0" },
                    backgroundColor = new[] { sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor, sLineFillColor },
                    borderColor = sLineFillColor,
                    yAxisID = "y0",
                    type = "line",
                    order = "0",
                    borderWidth = 2,
                    pointRadius = 2,
                }
            }
        };
    }
}