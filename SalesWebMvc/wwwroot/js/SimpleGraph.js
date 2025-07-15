const vendasPorMes = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(
    Model
        .GroupBy(v => new { v.Date.Year, v.Date.Month })
        .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
        .Select(g => new
            {
                Mes = $"{g.Key.Month:D2}/{g.Key.Year}",
                Total = g.Sum(v => v.Amount)
            })
));

const ctx = document.getElementById('graficoVendas').getContext('2d');
const chart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: vendasPorMes.map(v => v.Mes),
        datasets: [{
            label: 'Total de Vendas (R$)',
            data: vendasPorMes.map(v => v.Total),
            backgroundColor: 'rgba(0, 224, 224, 0.5)',
            borderColor: 'rgba(0, 224, 224, 1)',
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        indexAxis: 'y',
        scales: {
            x: {
                beginAtZero: true,
                ticks: {
                    callback: function (value) {
                        return 'R$ ' + value.toLocaleString('pt-BR');
                    }
                }
            }
        },
        plugins: {
            legend: {
                labels: {
                    font: {
                        size: 14
                    }
                }
            },
            tooltip: {
                callbacks: {
                    label: function (context) {
                        return 'R$ ' + context.raw.toFixed(2).replace('.', ',');
                    }
                }
            }
        }
    }
});