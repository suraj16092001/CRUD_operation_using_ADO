function validateData() {

    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var contactNo = document.getElementById("contactNo").value;
    var emailId = document.getElementById("emailId").value;
    var age = document.getElementById("age").value;

    var firstNameRegex = /^[a-zA-Z]+$/;
    var lastNameRegex = /^[a-zA-Z]+$/;
    var contactNoRegex = /^(?:\+91|91|0)?[789]\d[1-9]$/;
    var emailIdRegex = /^[0-9a-zA-Z]+[+._-]{0,1}[0-9a-zA-Z]+[@][a-zA-Z0-9]+[.][a-zA-Z]{2,3}([.][a-zA-Z]{2,3}){0,1}$/;
    var ageRegex = /^[1-9][0-9]$/;

    if (firstName == "") {

        $("#firstName_e").text("Please Provide First Name.").show();
    }
    else if (!firstNameRegex.test(firstName)) {
        $("#firstName_e").text("Please enter only characters.").show();
    }



    if (lastName == "") {

        $("#lastName_e").text("Please Provide Last Name.").show();
    }
    else if (!lastNameRegex.test(lastName)) {

        $("#lastName_e").text("Please Provide Last Name.").show();
    }


    if (contactNo == "") {

        $("#contactNo_e").text("Please enter only characters.").show();
    }
    else if (!contactNoRegex.test(contactNo)) {

        $("#contactNo_e").text("Please Enter valid contact Number.").show();
    }



    if (emailId == "") {

        $("#emailId_e").text("Please Provide Email ID.").show();
    }
    else if (!emailIdRegex.test(emailId)) {

        $("#emailId_e").text("Please Enter Valid Email ID").show();
    }


    if (age == "") {

        $("#age_e").text("Please Provide Age.").show();
    }
    else if (!ageRegex.test(age)) {

        $("#age_e").text("Please Enter valid age. ").show();
    }

}