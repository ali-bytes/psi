function ValidateEmail(textBoxControl)
{
    var email = textBoxControl.value
    var emailRegEx = /^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$/;
    textBoxControl.style.color = "white";
    if (emailRegEx.test(email)) {
        textBoxControl.style.backgroundColor = "#4DDB4D";
        document.getElementById("lblEmail").innerText = "";
    } else
    {
        textBoxControl.style.backgroundColor = "#FF4D4D";

    }
}

function ValidateNumeric(textBoxControl) {
    var number = textBoxControl.value
    var numberRegEx = /^[0-9]+$/;
    textBoxControl.style.color = "white";
    if (numberRegEx.test(number)) {
        textBoxControl.style.backgroundColor = "#4DDB4D";
        document.getElementById("lblMobile").innerText = "";
    } else {
        textBoxControl.style.backgroundColor = "#FF4D4D";

    }
}
function ValidateMoney(textBoxControl) {
    var value = textBoxControl.value
    var valueRegEx = /^([1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|\.[0-9]{1,2})$/;
    textBoxControl.style.color = "white";
    if (valueRegEx.test(value)) {
        textBoxControl.style.backgroundColor = "#4DDB4D";
        document.getElementById("lblSalary").innerText = "";
    } else {
        textBoxControl.style.backgroundColor = "#FF4D4D";

    }
}
function ValidateWebSite(textBoxControl) {
    var value1 = textBoxControl.value
    var expression = /[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?/gi;
    var valueRegEx = new RegExp(expression);
    textBoxControl.style.color = "white";
    if (valueRegEx.test(value1)) {
        textBoxControl.style.backgroundColor = "#4DDB4D";
        
    } else {
        textBoxControl.style.backgroundColor = "#FF4D4D";
        

    }
}
function ValidateName(textBoxControl) {
    var value = textBoxControl.value
    var valueRegEx = /^[a-zA-Z]+$/;
    textBoxControl.style.color = "white";
    if (valueRegEx.test(value)) {
        textBoxControl.style.backgroundColor = "#4DDB4D";
        document.getElementById("lblName").innerText = "";
        
    } else {
        textBoxControl.style.backgroundColor = "#FF4D4D";

    }
}
function ValidateDate(textBoxControl) {
    var value = textBoxControl.value
    var valueRegEx = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    textBoxControl.style.color = "white";
    if (valueRegEx.test(value)) {
        textBoxControl.style.backgroundColor = "#4DDB4D";
        document.getElementById("lblContDate").innerText = "";
    } else {
        textBoxControl.style.backgroundColor = "#FF4D4D";
    }
}

function ValidateForm(name)
{
    var ret = true;

    if (document.getElementById("txtName").value == "")
     {
          document.getElementById("lblName").innerText = "Name is required";
          ret = false;
      }
       else {
          document.getElementById("lblName").innerText = "";
         }

    if (document.getElementById("txtMobile").value == "") {
        document.getElementById("lblMobile").innerText = "Mobile is required";
        ret = false;
    }
    else {
        document.getElementById("lblMobile").innerText = "";
    }

    if (document.getElementById("txtEmail").value == "") {
        document.getElementById("lblEmail").innerText = "Email is required";
        ret = false;
    }
    else {
        document.getElementById("lblEmail").innerText = "";
    }

    if (document.getElementById("txtDate").value == "") {
        document.getElementById("lblContDate").innerText = "Date is required";
        ret = false;
    }
    else {
        document.getElementById("lblContDate").innerText = "";
    }

    if (document.getElementById("txtSalary").value == "") {
        document.getElementById("lblSalary").innerText = "SALARY is required";
        ret = false;
    }
    else {
        document.getElementById("lblSalary").innerText = "";
    }


    return ret;

}