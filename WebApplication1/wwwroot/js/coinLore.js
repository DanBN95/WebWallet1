$(document).ready(function () {
    $.ajax({
        url: "https://api.coinlore.net/api/tickers/",
        type: "GET",
        success: function (result) {
            var data = result["data"];
            for (var i = 0; i < data.length; i++) {
                var template = '<div class="coinCard"> <p> Coin Name: ' + data[i].name +
                    '</p><p> Coin Price: ' + data[i].price_usd +
                    '$</p><p> Change in last 24 hours: '+ data[i].percent_change_24h+'</p> </div>';
                $('#myDiv').append(template);
            }
            console.log(result);
            console.log(result.data.length);
            console.log(result["data"].length);


        },
        error: function (error) {
            console.log(error);
        }
    })

})