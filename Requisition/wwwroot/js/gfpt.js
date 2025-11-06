 
function showWarning(message) {
    Swal.fire({
        icon: 'warning',
        title: 'แจ้งเตือน',
        text: message,
        confirmButtonText: 'ตกลง'
    });
}
function showSuccess(message, redirectUrl = null, delay = 1500) {
    Swal.fire({
        icon: 'success',
        title: 'สำเร็จ',
        text: message,
        showConfirmButton: false,
        timer: delay
    }).then(() => {
        if (redirectUrl) {
            window.location.href = redirectUrl;
        }
    });
}
function showConfirm(message, confirmCallback) {
    Swal.fire({
        title: 'คุณแน่ใจหรือไม่?',
        text: message,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'ใช่, ดำเนินการ',
        cancelButtonText: 'ยกเลิก',
        reverseButtons: false // 👉 สลับตำแหน่ง Confirm กับ Cancel
    }).then((result) => {
        if (result.isConfirmed && typeof confirmCallback === 'function') {
            confirmCallback();
        }
    });
}

function Cancel() {
    var id = $('#RunningID').html();
    if (id == "") {
        Swal.fire({
            icon: "warning",
            title: "ไม่พบ เลขที่ใบแจ้ง!",
            confirmButtonText: "ตกลง"
        });
        return;
    }

    Swal.fire({
        title: "คุณแน่ใจ หรือไม่?",
        text: "เมื่อคุณยืนยันการเปลี่ยนข้อมูล รายการนี้จะถูกยกเลิก!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#ef5350",
        cancelButtonColor: "#6c757d",
        confirmButtonText: "ยืนยัน",
        cancelButtonText: "ยกเลิก"
    }).then((result) => {
        if (result.isConfirmed) {
            rejectC01(id);
        }
    });

    function rejectC01(id) {
        var url = URL + '/Requisition/Reject';
        $.ajax({
            type: "POST",
            url: url,
            data: { id: id },
            dataType: "json",
            beforeSend: function () { ShowLoading(); },
            success: function (res) {
                if (res.success === true) {
                    let link = URL + '/Requisition/List?statusPage=P01';

                    Swal.fire({
                        icon: "success",
                        title: "สำเร็จ!",
                        text: "ยกเลิกรายการเรียบร้อยแล้ว",
                        confirmButtonText: "ตกลง"
                    }).then(() => {
                        window.location.href = link;
                    });
                }
            },
            complete: function () {
                ShowLoading(false);
            },
            error: function (ex) {
                Swal.fire({
                    icon: "error",
                    title: "เกิดข้อผิดพลาด",
                    text: "ไม่สามารถยกเลิกรายการได้"
                });
            }
        });
    }
}

