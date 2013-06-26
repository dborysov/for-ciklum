var ReportsManager = (function () {
    var getFiltersObject = function() {
        return {
            projectId: +$('#Projects').val() == 0 ? null : +$('#Projects').val(),
            period: +$('#Periods').val(),
            endDate: $('#EndDate').val(),
        };
    };

    var reloadReport = function () {
        $.ajax({
            url: "/Reports/LoadReport",
            data: getFiltersObject(),
            success: function (data) {
                $('#report_table_wrapper').html(data);
                bindShareReportButton();
            }
        });
    };

    var bindShareReportButton = function() {
        $('#report_share button').click(function () {
            $(this).attr('disabled', true);
            $.ajax({
                url: "/Reports/ShareReport",
                data: getFiltersObject(),
                success: function (data) {
                    $('#report_share').html(data).find('input').select().click(function() {
                        this.select();
                    });
                }
            });
        });
    };

    var bindEvents = function() {
        $('#Projects, #Periods, #EndDate').change(function () { reloadReport(); });
        return this;
    };

    return {
        bindEvents: bindEvents,
        reloadReport: reloadReport
    };
})();

$(ReportsManager.bindEvents().reloadReport());