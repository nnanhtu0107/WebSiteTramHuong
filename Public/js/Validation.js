function getValue(id) {
    return document.getElementById(id).value.trim();
}
// Hiển thị lỗi
function showError(key, mess) {
    document.getElementById(key + '_error').innerHTML = mess;
}
function validate() {
    var flag = true;
    var TenKH = getValue("TenKH");
    if (TenKH == "") {
        flag = false;
        showError("TenKH", "Vui lòng nhập Tên");
    }
    var DiaChi = getValue("DiaChi");
    if (DiaChi == "") {
        flag = false;
        showError("DiaChi", "Vui lòng nhập Địa Chỉ");
    }
    var SoDienThoai = getValue("SoDienThoai");
    if (SoDienThoai != "" && !/^[0-9]{10}$/.test(SoDienThoai)) {
        flag = false;
        showError("SoDienThoai", "Vui lòng kiểm tra lại Số Điện Thoại");
    }
    var Email = getValue("Email");
    var mailformat = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (!mailformat.test(Email)) {
        flag = false;
        showError("Email", "Vui lòng kiểm tra lại Email");
    }
    return flag;
}
