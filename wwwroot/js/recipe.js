var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#recipeTable').DataTable({
        "ajax": {
            "url": "/Admin/Recipe/GetAll"
        },
        "columns": [

            { "data": "title", "width": "25%" },
            { "data": "applicationUser", "width": "25%" },
            {
                "data": "created",
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return date.getDate() + "/" + (month.toString().length > 1 ? month : "0" + month) + "/" + date.getFullYear();
                },"width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Recipe/Upsert?id=${data}"
                            class="btn btn-primary mx-2" 
                            style="border-top-right-radius: 5px; border-bottom-right-radius: 5px;">
                            <i class="bi bi-pencil"></i> Edit
                        </a>
                            <button type="submit" class="btn btn-outline-danger mx-2"
                                <a onClick=Delete('/Admin/Recipe/Delete/${data}')
                                data-confirm="Are you sure you want to delete this difficulty level?">
                                <i class="bi bi-x-lg"></i>
                                Delete
                            </button>
                        
                    </div>
                    `
                },
                "width": "25%"
},
        ]
        
    })
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}