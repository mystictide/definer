function toggle() {
    document.getElementById('settings-dropdown').classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.settings-toggle')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

var sidebar = {
    fill: function () {
        $.ajax({
            url: "/sideBar",
            success: function (data) {
                localStorage.content = data;
                $('#s-content').prepend(data);
                return data;
            }
        });
    }
}