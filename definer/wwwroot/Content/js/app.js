$(document).ready(
    function () {
        sidebar.fill();
    },
);

$('#sbutton').attr('disabled', true);
$('#search').on('input change', function () {
    if ($(this).val().length != 0)
        $('#sbutton').attr('disabled', false);
    else
        $('#sbutton').attr('disabled', true);
});

$(document).bind('change', '.spages', function () {
    var value = $('.spages').val();
    sidebar.filter(value);
});

//$('.spages').on('change', function () {
//    var value = $(this).val();
//    console.log(value);
//    sidebar.filter(value);
//});

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

var authorpage = {
    fill: function () {
        var username = $("#author").val();
        var data = { username: username };
        $.ajax({
            url: "/u/authorEntries",
            data: data,
            success: function (data) {
                $('#rss-entries').empty().prepend(data);
            }
        });
    },
    filter: function (page) {
        var username = $("#author").val();
        var filter = { page: page };
        $.ajax({
            url: "/u/authorEntries",
            data: { filter: filter, username: username },
            success: function (data) {
                $('#rss-entries').empty().prepend(data);
            }
        });
    }
}

var sidebar = {
    fill: function () {
        $.ajax({
            url: "/sideBar",
            success: function (data) {
                localStorage.content = data;
                $('#s-content').empty().prepend(data);
                //return data;
            }
        });
    },
    filter: function (page) {
        var filter = {page: page};
        $.ajax({
            url: "/sideBar",
            data: filter,
            success: function (data) {
                localStorage.content = data;
                $('#s-content').empty().prepend(data);
                //return data;
            }
        });
    }
}

var entryAttribute = {
    voteUP: function (EntryID) {
        var model = { EntryID: EntryID, Vote: 'True' };
        $.ajax({
            url: "/vote",
            data: model,
            success: function (model) {
                if (model == null || model.vote == 0 || model.vote == null) {
                    $("#def" + EntryID).find(".downvoted").removeClass("active");
                    $("#def" + EntryID).find(".upvoted").removeClass("active");
                    if (model.upvotes < 1) {
                        $("#def" + EntryID).find(".upCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".upCount").text(model.upvotes);
                    }
                    if (model.downvotes < 1) {
                        $("#def" + EntryID).find(".downCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".downCount").text(model.downvotes);
                    }
                }
                else if (model.vote == 1) {
                    $("#def" + EntryID).find(".upvoted").toggleClass("active");
                    $("#def" + EntryID).find(".downvoted").removeClass("active");
                    if (model.upvotes < 1) {
                        $("#def" + EntryID).find(".upCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".upCount").text(model.upvotes);
                    }
                    if (model.downvotes < 1) {
                        $("#def" + EntryID).find(".downCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".downCount").text(model.downvotes);
                    }
                }
            }
        });
    },
    voteDOWN: function (EntryID) {
        var model = { EntryID: EntryID, Vote: 'False' };
        $.ajax({
            url: "/vote",
            data: model,
            success: function (model) {
                if (model == null || model.vote == 1 || model.vote == null) {
                    $("#def" + EntryID).find(".downvoted").removeClass("active");
                    $("#def" + EntryID).find(".upvoted").removeClass("active");
                    if (model.upvotes < 1) {
                        $("#def" + EntryID).find(".upCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".upCount").text(model.upvotes);
                    }
                    if (model.downvotes < 1) {
                        $("#def" + EntryID).find(".downCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".downCount").text(model.downvotes);
                    }
                }
                else if (model.vote == 0) {
                    $("#def" + EntryID).find(".downvoted").toggleClass("active");
                    $("#def" + EntryID).find(".upvoted").removeClass("active");
                    if (model.upvotes < 1) {
                        $("#def" + EntryID).find(".upCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".upCount").text(model.upvotes);
                    }
                    if (model.downvotes < 1) {
                        $("#def" + EntryID).find(".downCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".downCount").text(model.downvotes);
                    }
                }
            }
        });
    },
    fav: function (EntryID) {
        var model = { EntryID: EntryID };
        $.ajax({
            url: "/fav",
            data: model,
            success: function (model) {
                if (model.favourite) {
                    $("#def" + EntryID).find(".fav").toggleClass("active");
                    if (model.favourites < 1) {
                        $("#def" + EntryID).find(".favCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".favCount").text(model.favourites);
                    }
                }
                else if (!model.favourite) {
                    $("#def" + EntryID).find(".fav").removeClass("active");
                    if (model.favourites < 1) {
                        $("#def" + EntryID).find(".favCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".favCount").text(model.favourites);
                    }
                }
                else if (model.favourite == null) {
                    $("#def" + EntryID).find(".fav").removeClass("active");
                    if (model.favourites < 1) {
                        $("#def" + EntryID).find(".favCount").text('');
                    }
                    else {
                        $("#def" + EntryID).find(".favCount").text(model.favourites);
                    }
                }
            }
        });
    }
}

var textEditor = {
    checkSelection: function (e) {
        var textarea = document.getElementById("Body");
        var len = textarea.value.length;
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        var sel = textarea.value.substring(start, end);

        if (e.target.id == "bthread") {
            if (sel < 1) {
                var val = textEditor.promt("which thread did you mean?");
                if (val.length > 0) {
                    textEditor.appendThread(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapThread(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bspoiler") {
            if (sel < 1) {
                var val = textEditor.promt("what do we not spoil?");
                if (val.length > 0) {
                    textEditor.appendSpoiler(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapSpoiler(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "blink") {
            if (sel < 1) {
                var val = textEditor.promt("enter some link");
                if (val.length > 0) {
                    textEditor.appendLink(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapLink(textarea, len, start, end, sel);
            }
        }
    },

    promt: function (q) {
        return prompt(q);
    },

    appendThread: function (textarea, len, start, end, val) {
        var replace = '[thread ' + val + ']';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapThread: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[thread ' + sel + ']';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendLink: function (textarea, len, start, end, val) {
        var replace = '[link ' + val + ']';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapLink: function (textarea, len, start, end, sel) {
        var replace = '[link ' + sel + ']';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendSpoiler: function (textarea, len, start, end, val) {
        var replace = '[spoiler ' + val + ']';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapSpoiler: function (textarea, len, start, end, sel) {
        var replace = '[spoiler ' + sel + ']';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    }
}

var Filter = {

    Page: function (page) {
        var params = Filter.querystring.add("page", page, true);
        Filter.querystring.redirect(params);
    },

    Clear: function () {
        let querystring = Filter.querystring.read();
        KodLoading.refresh(window.location.pathname + "?keyword=" + querystring.get('keyword'));
    },

    querystring: {
        read: function () {
            return new URLSearchParams(window.location.search);
        },
        add: function (key, value, isChange) {
            var urlParams = Filter.querystring.read();
            if (isChange) {
                urlParams.delete(key);
                urlParams.append(key, value);
            } else {
                var newParams = new URLSearchParams();

                var control = false;
                for (pair of urlParams.entries()) {
                    if (pair[0] == key & pair[1] == value) {
                        control = true;
                    } else {
                        newParams.append(pair[0], pair[1]);
                    }
                }
                if (!control) {
                    newParams.append(key, value);
                }
                urlParams = newParams;
            }
            return urlParams;
        },
        redirect: function (newParams) {
            Filter.refresh(window.location.pathname + "?" + decodeURIComponent(newParams.toString()));
        }
    },

    Search: function (button) {
        var form = $(button).parents('form');
        var url = window.location.pathname;
        var first = true;

        $('input', form).each(function (index, item) {

            var key = $(item).attr('name');
            var value = $(item).val();
            if (!(!value)) {
                var _icon = first ? "?" : "&";
                url += _icon + key + "=" + value;
                first = false;
            }
        });

        $('.param.active', form).each(function (index, item) {
            var key = $(item).attr('name');
            var value = $(item).data('id');
            if (!(!value) || value == 0) {
                var _icon = first ? "?" : "&";
                url += _icon + key + "=" + value;
                first = false;
            }

        });

        Filter.refresh(url);
    },

    refresh: function (url) {

        if (url == undefined || url == null) {
            location.reload();
        } else {
            window.location.href = url;
        }
    },
}