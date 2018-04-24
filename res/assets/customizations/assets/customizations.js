function setCopyrightYear() {
  var date = new Date();
  var currentYear = date.getFullYear();
  var copyrightYear = document.getElementById("copyrightYear");
  copyrightYear.innerHTML = currentYear;
}

function setFeedbackLink() {
    var theLink = document.getElementById("feedbackLink");
    var linkStr = "mailto:doc-feedback@ooyala.com?Subject=Feedback%20%7C%20Welcome&body=I%20have%20a%20comment%20for%20the%20page%20%0A%0A%20";
    linkStr += encodeURIComponent(window.location);
    linkStr += "%20%0A%0A%3Cyour%20comment%20here%3E";
    theLink.href = linkStr;
}
