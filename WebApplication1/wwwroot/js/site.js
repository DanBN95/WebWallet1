
//$(document).ready(function () {
//    $('#myTable').DataTable({
//        "scrollY": "450px",
//        "scrollCollapse": true,
//        "paging": true
//    });
//});

function setForm(value) {

    if (value == 'form1') {
        document.getElementById('form1').style = 'display:block;';
        document.getElementById('form2').style = 'display:none;';
    }
    else {

        document.getElementById('form2').style = 'display:block;';
        document.getElementById('form1').style = 'display:none;';
    }

}

function setfuns() {
    setInterval('startTime()', 360000);
    
}



        function startTime()
    {
         var today = new Date();
            var day = today.getDate().toString();
            var m = today.getMonth().toString();
            var y = today.getFullYear().toString();
            day = checkTime(day);
            m = checkTime(m);
            document.getElementById('time').innerHTML = day+"."+m+"."+y  ;
               }

    function checkTime(i)
    {
        if (i<10)
    {
        i = "0" + i; 
    }
    return i; 
}

function getdate() {
    var today = new Date();
    return today;
}

function Sort(value) {
    console.log("Hello World");
}

document.addEventListener('click', () => {
    const map = new Map();
    
})


