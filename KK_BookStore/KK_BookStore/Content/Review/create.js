$(document).ready(function () {


    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.avatar').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }


    $(".file-upload").on('change', function () {
        readURL(this);
    });
});
$(function () {
    $('#btnUpload').click(function () {
        $('#fileUpload').trigger('click');
    });
});
$('#fileUpload').change(function () {
    if (window.FormData !== undefined) {
        var fileUpload = $('#fileUpload').get(0);
        var files = fileUpload.files;
        var formData = new FormData();
        formData.append('file', files[0]);
        $.ajax(
            {
                type: 'POST',
                url: '/Review/ProcessUpload',
                contentType: false,
                processData: false,
                data: formData,
                success: function (urlImage) {
                    $('#pictureUpload').attr('src', urlImage);
                    $('#AnhBia').val(urlImage);
                },
                error: function (err) {
                    alert('Error ', err.statusText);
                    19
                }
            });
    }
});
