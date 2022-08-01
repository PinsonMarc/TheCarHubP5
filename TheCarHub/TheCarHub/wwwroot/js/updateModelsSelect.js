$(function () {
    $("#Model_MakeId").on('change', function () {
        if (!this.value) {
            $("#ModelId").empty();
            return
        }
        $.ajax({
            type: "GET",
            url: "/Admin/GetModels/" + this.value,
            dataType: "json",
            success: function (result) {
                console.log(result);
                var models = JSON.parse(result.value);
                var newOptions = {};
                for (let i of models) {
                    newOptions[i.Name] = i.Id;
                }

                var $el = $("#ModelId");
                $el.empty(); // remove old options
                $.each(newOptions, function (key, value) {
                    $el.append($("<option></option>").attr("value", value).text(key));
                });
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    });
    //$(".custom-file-input").on("change", function () {
    //    var fileName = $(this).val().split("\\").pop();
    //    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    //});
});