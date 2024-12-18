function DeleteWarning(url, table) {
    Swal.fire({
        title: 'Are you sure you want delete?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            'Deleted!',
                            'Deleted successfully',
                            'success'
                        );

                        $(table).load(window.location.href + table);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Unable to delete!'
                        })
                    }
                }
            })
        }
    })
}