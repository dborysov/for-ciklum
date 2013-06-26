var ProjectsManager = (function () {

    var bindEdit = function () {
        $('div#projects-list table.list td.tools .edit').click(function () {
            $.ajax({
                url: "/Projects/EditProject",
                data: { projectName: $(this).parent().siblings('.project-name').text() },
                success: function (data) {
                    if (data != null && data != '') {
                        $('#g_ajax_popup').html(data).show().draggable({ containment: "html", scroll: false });
                        $.validator.unobtrusive.parse($('#edit_project_postback'));
                    }
                }
            });
        });
    };

    var bindDeleteSingle = function () {
        $('div#projects-list table.list td.tools .delete').click(function () {
            if (confirm("Are you shure?")) {
                $.ajax({
                    url: "/Projects/DeleteProject",
                    data: { projectName: $(this).parent().siblings('.project-name').text() },
                    type: "Post",
                    success: function (data) {
                        $('#projects-list').html(data);
                        bindTableEvents();
                    }
                });
            }
        });
    };

    var bindDeleteMultiple = function () {

        $('#delete_project').click(function () {
            var checkedProjectsNames = $('div#projects-list table.list td.project-checker input:checkbox:checked').parent() //td
                                                                                                                  .siblings('.project-name') //taking name from the same tr
                                                                                                                  .toArray()
                                                                                                                  .map(function (a) { return $(a).text(); });

            $.ajax({
                url: "/Projects/DeleteMultipleProjects",
                data: { projectNames: checkedProjectsNames },
                type: "Post",
                datatype: "json",
                traditional: true,
                success: function (data) {
                    $('#projects-list').html(data);
                    bindEvents();
                }
            });
        });
    };

    var bindCheckAll = function () {
        $('div#projects-list table.list th input#check_all_projects:checkbox').change(function () {
            var checkboxes = $('div#projects-list table.list td.project-checker input:checkbox');
            if ($(this).attr('checked')) {
                checkboxes.attr('checked', 'checked').trigger('change');
            } else {
                checkboxes.removeAttr('checked').trigger('change');
            }
        });
    };

    var bindDeleteButtonDisableChanging = function () {
        $('div#projects-list table.list td.project-checker input:checkbox').change(
            function () {
                deleteButtonDisableChange();
                selectAllCheckedChange();
            }
        );
    };

    var selectAllCheckedChange = function() {
        if ($('div#projects-list table.list td.project-checker input:checkbox').toArray().every(function(a) { return $(a).attr('checked'); })) {
            $('div#projects-list table.list th input#check_all_projects:checkbox').attr("checked", "checked");
        } else {
            $('div#projects-list table.list th input#check_all_projects:checkbox').removeAttr("checked");
        }
    };

    var deleteButtonDisableChange = function () {
        if ($('div#projects-list table.list td.project-checker input:checkbox').toArray().some(function (a) { return $(a).attr('checked'); })) {
            $('#delete_project').removeAttr('disabled');
        } else {
            $('#delete_project').attr('disabled', 'disabled');
        }
    };

    var bindTableEvents = function() {
        bindEdit();
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

$(ProjectsManager.bindEvents());