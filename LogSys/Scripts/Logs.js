var LogsManager = (function () {
    var bindDeleteSingle = function () {
        $('div#logs-list table.list td.tools .delete').click(function () {
            if (confirm("Are you shure?")) {
                $.ajax({
                    url: "/Logs/DeleteLog",
                    data: { logId: $(this).parent().parent().attr('data-log-id') },
                    type: "Post",
                    success: function (data) {
                        $('#logs-list').html(data);
                        bindTableEvents();
                    }
                });
            }
        });
    };

    var bindDeleteMultiple = function () {

        $('#delete_log').click(function () {
            var checkedLogsIds = $('div#logs-list table.list td.log-checker input:checkbox:checked').parent() //td
                                                                                                    .parent() //taking name from the same tr
                                                                                                    .toArray()
                                                                                                    .map(function(a) { return $(a).attr('data-log-id'); });

            $.ajax({
                url: "/Logs/DeleteMultipleLogs",
                data: { LogsIds: checkedLogsIds },
                type: "Post",
                datatype: "json",
                traditional: true,
                success: function (data) {
                    $('#logs-list').html(data);
                    bindEvents();
                }
            });
        });
    };

    var bindCheckAll = function () {
        $('div#logs-list table.list th input#check_all_logs:checkbox').change(function () {
            var checkboxes = $('div#logs-list table.list td.log-checker input:checkbox');
            if ($(this).attr('checked')) {
                checkboxes.attr('checked', 'checked').trigger('change');
            } else {
                checkboxes.removeAttr('checked').trigger('change');
            }
        });
    };

    var bindDeleteButtonDisableChanging = function () {
        $('div#logs-list table.list td.log-checker input:checkbox').change(
            function () {
                deleteButtonDisableChange();
                selectAllCheckedChange();
            }
        );
    };

    var selectAllCheckedChange = function () {
        if ($('div#logs-list table.list td.log-checker input:checkbox').toArray().every(function (a) { return $(a).attr('checked'); })) {
            $('div#logs-list table.list th input#check_all_logs:checkbox').attr("checked", "checked");
        } else {
            $('div#logs-list table.list th input#check_all_logs:checkbox').removeAttr("checked");
        }
    };

    var deleteButtonDisableChange = function () {
        if ($('div#logs-list table.list td.log-checker input:checkbox').toArray().some(function (a) { return $(a).attr('checked'); })) {
            $('#delete_log').removeAttr('disabled');
        } else {
            $('#delete_log').attr('disabled', 'disabled');
        }
    };

    var bindTableEvents = function () {
        bindDeleteSingle();
        bindCheckAll();
        bindDeleteButtonDisableChanging();
    };

    var bindEvents = function () {
        bindTableEvents();
        bindDeleteMultiple();
    };

    return {
        bindEvents: bindEvents,
        bindTableEvents: bindTableEvents
    };
})();

$(LogsManager.bindEvents());