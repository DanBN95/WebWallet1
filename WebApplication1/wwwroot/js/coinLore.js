$(document).ready(function () {
 
        $.ajax({
            url: "https://api.coinlore.net/api/tickers/",
            type: "GET",
            success: function (result) {
                var data = result["data"];
                for (var i = 0; i < data.length; i++) {
                    var template =
                        '<div class="card"> <i class="fab fa-bitcoin card-img-top"></i> <div class="card-body"> <h5 class="card-title">' + data[i].name +
                        '</h5> <p class="card-text">Coin Price: ' + data[i].price_usd +
                        '</p> <p class="card-text">  Change in last 24 hours: ' + data[i].percent_change_24h +
                        '</p> </div> </div>';
                        
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


$(function () {
$('.apiBtn').click(function () {
    alert("hey");
})
})