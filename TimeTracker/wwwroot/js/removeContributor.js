
function removeContributor(clickedElem) {
    var parent = $(clickedElem).closest(".form-group");
    var email = parent.find('.form-control').val();
    var $formData = new FormData(document.getElementById("form"));
    $formData.append('contributorEmail', email);
    $.ajax({
        async: true,
        data: $formData,
        processData: false,
        contentType: false,
        dataType: 'HTML',
        type: "POST",
        url: '/Projects/RemoveContributor',
        success: function (partialView) {
            console.log("partialView: " + partialView);
            $('#contributorsContainer').html(partialView);
        }
    });
};