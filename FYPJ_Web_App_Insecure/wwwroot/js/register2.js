function myFunction() {
    Message = document.getElementById('Message').value;
    var xml = "<?xml version='1.0'?><query><message>" + Message + "</message></query>";
    alert(Message);
    $.ajax({
        type: "POST",
        url: '/XML/Index',
        data: { xml: xml },
        dataType: "text",
        success: function (msg) {
            console.log("It was succesfull");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
};