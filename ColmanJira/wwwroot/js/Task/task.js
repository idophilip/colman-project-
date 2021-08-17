$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get('typeId') !== null && urlParams.get('typeId') !== '') {
        $('#task-type-id').val(urlParams.get('typeId'));
    }

    if (urlParams.get('statusId') !== null && urlParams.get('statusId') !== '') {
        $('#task-status-id').val(urlParams.get('statusId'));
    }

    $.get('/Tasks/TasksCountByType', (data) => {
        const mostUsed = { name: '', value: '' };

        let max = 0;

        for (var currData of data) {
            if (currData.value > max) {
                max = currData.value;
                mostUsed.name = currData.name;
                mostUsed.value = currData.value;
            }
        }

        $("#type-select option").each(function () {
            if ($(this).text() === mostUsed.name) {
                $(this).text(`${$(this).text()} - Recommended! ☆`);
            }
        });
    });
});

function fillValues(title, desc, id, statusId, typeId, userId) {
    $('#task-title').val(title);
    $('#task-desc').val(desc);
    $('#task-id').val(id);
    $('#status-id').val(statusId);
    $('#type-id').val(typeId);
    $('#task-status').val(statusId);
    $('#task-type').val(typeId);
    $('#task-user').val(userId);
}

function addTask() {
}

function updateTask() {
}

function filter(taskTypeId, taskStatusId) {
    /*$.get(`/Tasks/Filter?typeId=${taskTypeId}&statusId=${taskStatusId}`, (tasks) => {

    });*/
}
