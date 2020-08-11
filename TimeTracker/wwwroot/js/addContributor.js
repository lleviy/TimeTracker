function addContributor () {
    $.ajax({
        async: true,
        data: $('#form').serialize(),
        type: "POST",
        url: '/TaskTypes/AddContributor',
        success: function (partialView) {
            console.log("partialView: " + partialView);
            $('#contributorsContainer').html(partialView);
        }
    });
};