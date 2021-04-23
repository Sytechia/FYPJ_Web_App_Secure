function myFunction() {
    Name = document.getElementById('Username').value
    email = document.getElementById('Pwd').value
    var xml = "<?xml version='1.0'?><query><name>" + Name + "</name><email>" + email + "</email></query>";
    $.ajax({
        type: "POST",
        url: '/Account/Create',
        data: { xml: xml },
        dataType: "text",
        success: function (msg) {
            console.log("It was succesfull");
            console.log(data["number1"]);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
};