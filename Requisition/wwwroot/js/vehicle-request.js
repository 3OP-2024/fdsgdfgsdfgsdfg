// ===== CONFIG =====

// สมมติ ProvinceId ที่นับเป็น "กรุงเทพและปริมณฑล"
// ต้องปรับให้ตรงกับ Master ของจริง (id หรือ code ก็ได้)
const metroProvinceIds = [10, 11, 12, 13, 14];
// ตัวอย่าง: 10 = กทม., 11 = นนทบุรี, 12 = ปทุมฯ, 13 = สมุทรปราการ, 14 = สมุทรสาคร

$(document).ready(function () {

    // โหลดรายการครั้งแรก
    loadRequests();

    // ปุ่มค้นหา
    $("#btnSearch").on("click", function () {
        loadRequests();
    });

    // ปุ่มสร้างคำขอใหม่
    $("#btnCreateRequest").on("click", function () {
        openCreateModal();
    });

    // เปลี่ยนจังหวัดแล้วเช็คข้อความ "ต้องผ่านการอนุมัติ 3"
    $("#ProvinceId").on("change", function () {
        checkProvinceApprove3();
    });

    // เพิ่ม-ลบ ผู้โดยสาร
    $("#btnAddPassenger").on("click", function () {
        addPassengerRow();
    });
    $("#tblPassengers").on("click", ".btn-remove-passenger", function () {
        $(this).closest("tr").remove();
        renumberRows("#tblPassengers");
    });

    // เพิ่ม-ลบ จุดขึ้นรถ
    $("#btnAddPickup").on("click", function () {
        addPickupRow();
    });
    $("#tblPickups").on("click", ".btn-remove-pickup", function () {
        $(this).closest("tr").remove();
        renumberRows("#tblPickups");
    });

    // ปุ่มบันทึก
    $("#btnSaveRequest").on("click", function () {
        saveRequest();
    });

    // คลิกดูลายละเอียดจาก table
    $("#tblRequests").on("click", ".btn-edit", function () {
        const id = $(this).data("id");
        openEditModal(id);
    });
});

// ====== Load list ======
function loadRequests() {
    const depId = $("#DepartmentId").val() || "";
    const dateFrom = $("#UseDateFrom").val() || "";
    const dateTo = $("#UseDateTo").val() || "";

    $.ajax({
        url: '/VehicleRequest/Search',
        type: 'GET',
        data: {
            departmentId: depId,
            useDateFrom: dateFrom,
            useDateTo: dateTo
        },
        success: function (res) {
            // res = list ของคำขอ
            const $tbody = $("#tblRequests tbody");
            $tbody.empty();

            if (!res || res.length === 0) {
                $tbody.append(`<tr><td colspan="7" class="text-center text-muted">ไม่พบข้อมูล</td></tr>`);
                return;
            }

            $.each(res, function (i, item) {
                const row = `
                <tr>
                    <td>${i + 1}</td>
                    <td>${item.requestNo || ''}</td>
                    <td>${formatDateRange(item.useDateTimeFrom, item.useDateTimeTo)}</td>
                    <td>${item.departmentName || ''}</td>
                    <td>${item.locationName || ''}</td>
                    <td>${item.statusName || ''}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-sm btn-outline-primary btn-edit" data-id="${item.id}">
                            รายละเอียด
                        </button>
                    </td>
                </tr>`;
                $tbody.append(row);
            });
        },
        error: function (xhr) {
            console.error(xhr);
            alert("ไม่สามารถดึงข้อมูลรายการคำขอได้");
        }
    });
}

function formatDateRange(fromStr, toStr) {
    if (!fromStr || !toStr) return "";
    // แปลง string -> Date แล้ว format ตามที่ต้องการ
    const fromDt = new Date(fromStr);
    const toDt = new Date(toStr);
    return fromDt.toLocaleString('th-TH') + ' - ' + toDt.toLocaleString('th-TH');
}

// ====== Modal: Create / Edit ======
function openCreateModal() {
    clearRequestForm();
    $("#modal-title-text").text("สร้างคำขอใช้งานรถ");
    const modal = new bootstrap.Modal(document.getElementById('modal-request'));
    modal.show();
}

function openEditModal(id) {
    clearRequestForm();
    $("#modal-title-text").text("รายละเอียดคำขอใช้งานรถ");

    $.ajax({
        url: '/VehicleRequest/Get',
        type: 'GET',
        data: { id: id },
        success: function (res) {
            if (!res) return;

            $("#RequestId").val(res.id);
            $("#UseDateTimeFrom").val(toInputDateTimeValue(res.useDateTimeFrom));
            $("#UseDateTimeTo").val(toInputDateTimeValue(res.useDateTimeTo));
            $("#RequestDepartmentId").val(res.departmentId);
            $("#LocationName").val(res.locationName);
            $("#ProvinceId").val(res.provinceId);
            $("#DistrictName").val(res.districtName);
            $("#LocationDetail").val(res.locationDetail);
            $("#Purpose").val(res.purpose);

            checkProvinceApprove3();

            // ผู้โดยสาร
            if (res.passengers && res.passengers.length > 0) {
                $.each(res.passengers, function (i, p) {
                    addPassengerRow(p.fullName, p.remark);
                });
            }

            // จุดขึ้นรถ
            if (res.pickups && res.pickups.length > 0) {
                $.each(res.pickups, function (i, p) {
                    addPickupRow(p.pickupType, p.description);
                });
            }

            // อนุมัติ
            $("#Approve1By").val(res.approve1By);
            $("#Approve1DateTime").val(formatThaiDateTime(res.approve1DateTime));
            $("#Approve1Remark").val(res.approve1Remark);

            $("#Approve2By").val(res.approve2By);
            $("#Approve2DateTime").val(formatThaiDateTime(res.approve2DateTime));
            $("#Approve2Remark").val(res.approve2Remark);

            $("#Approve3By").val(res.approve3By);
            $("#Approve3DateTime").val(formatThaiDateTime(res.approve3DateTime));
            $("#Approve3Remark").val(res.approve3Remark);

            const modal = new bootstrap.Modal(document.getElementById('modal-request'));
            modal.show();
        },
        error: function (xhr) {
            console.error(xhr);
            alert("ไม่สามารถดึงรายละเอียดคำขอได้");
        }
    });
}

function clearRequestForm() {
    $("#formRequest")[0].reset();
    $("#RequestId").val("");
    $("#tblPassengers tbody").empty();
    $("#tblPickups tbody").empty();
    $("#province-approve3-text").addClass("d-none");
}

// ===== Province Rule: แสดงข้อความต้องผ่านอนุมัติ 3 =====
function checkProvinceApprove3() {
    const val = $("#ProvinceId").val();
    const show = val && metroProvinceIds.indexOf(parseInt(val)) === -1;
    if (show) {
        $("#province-approve3-text").removeClass("d-none");
    } else {
        $("#province-approve3-text").addClass("d-none");
    }
}

// ===== Dynamic Rows: Passengers =====
function addPassengerRow(fullName, remark) {
    const $tbody = $("#tblPassengers tbody");
    const index = $tbody.find("tr").length + 1;

    const row = `
        <tr>
            <td class="text-center row-no">${index}</td>
            <td>
                <input type="text" class="form-control passenger-name" placeholder="ชื่อ-นามสกุล" value="${fullName || ''}" required />
            </td>
            <td>
                <input type="text" class="form-control passenger-remark" placeholder="หมายเหตุ" value="${remark || ''}" />
            </td>
            <td class="text-center">
                <button type="button" class="btn btn-link text-danger btn-remove-passenger">
                    <i class="ti ti-x"></i>
                </button>
            </td>
        </tr>`;
    $tbody.append(row);
}

// ===== Dynamic Rows: Pickup Points =====
function addPickupRow(pickupType, description) {
    const $tbody = $("#tblPickups tbody");
    const index = $tbody.find("tr").length + 1;

    const row = `
        <tr>
            <td class="text-center row-no">${index}</td>
            <td>
                <select class="form-select pickup-type">
                    <option value="Company" ${pickupType === 'Company' ? 'selected' : ''}>บริษัท</option>
                    <option value="Other" ${pickupType === 'Other' ? 'selected' : ''}>จุดอื่น</option>
                </select>
            </td>
            <td>
                <input type="text" class="form-control pickup-desc" placeholder="รายละเอียดจุดขึ้นรถ" value="${description || ''}" required />
            </td>
            <td class="text-center">
                <button type="button" class="btn btn-link text-danger btn-remove-pickup">
                    <i class="ti ti-x"></i>
                </button>
            </td>
        </tr>`;
    $tbody.append(row);
}

// ===== Utility =====
function renumberRows(tableSelector) {
    $(tableSelector + " tbody tr").each(function (i, el) {
        $(el).find(".row-no").text(i + 1);
    });
}

function toInputDateTimeValue(str) {
    if (!str) return "";
    const dt = new Date(str);
    // yyyy-MM-ddTHH:mm
    const pad = (n) => n.toString().padStart(2, '0');
    return dt.getFullYear() + "-" +
        pad(dt.getMonth() + 1) + "-" +
        pad(dt.getDate()) + "T" +
        pad(dt.getHours()) + ":" +
        pad(dt.getMinutes());
}

function formatThaiDateTime(str) {
    if (!str) return "";
    const dt = new Date(str);
    return dt.toLocaleString('th-TH');
}

// ===== Save =====
function saveRequest() {
    // validate เบื้องต้น
    if (!$("#UseDateTimeFrom").val() || !$("#UseDateTimeTo").val() || !$("#RequestDepartmentId").val()) {
        alert("กรุณากรอกข้อมูลวันที่และฝ่าย/แผนกให้ครบ");
        return;
    }

    // เก็บข้อมูล header
    const header = {
        id: $("#RequestId").val() || 0,
        useDateTimeFrom: $("#UseDateTimeFrom").val(),
        useDateTimeTo: $("#UseDateTimeTo").val(),
        departmentId: $("#RequestDepartmentId").val(),
        locationName: $("#LocationName").val(),
        provinceId: $("#ProvinceId").val(),
        districtName: $("#DistrictName").val(),
        locationDetail: $("#LocationDetail").val(),
        purpose: $("#Purpose").val()
    };

    // ผู้โดยสาร
    const passengers = [];
    $("#tblPassengers tbody tr").each(function () {
        const name = $(this).find(".passenger-name").val();
        const remark = $(this).find(".passenger-remark").val();
        if (name) {
            passengers.push({
                fullName: name,
                remark: remark
            });
        }
    });

    // จุดขึ้นรถ
    const pickups = [];
    $("#tblPickups tbody tr").each(function () {
        const type = $(this).find(".pickup-type").val();
        const desc = $(this).find(".pickup-desc").val();
        if (desc) {
            pickups.push({
                pickupType: type,
                description: desc
            });
        }
    });

    const payload = {
        header: header,
        passengers: passengers,
        pickups: pickups
    };

    $.ajax({
        url: '/VehicleRequest/Save',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(payload),
        success: function (res) {
            alert("บันทึกข้อมูลเรียบร้อย");
            const modalEl = document.getElementById('modal-request');
            const modal = bootstrap.Modal.getInstance(modalEl);
            modal.hide();
            loadRequests();
        },
        error: function (xhr) {
            console.error(xhr);
            alert("ไม่สามารถบันทึกคำขอได้");
        }
    });
}
