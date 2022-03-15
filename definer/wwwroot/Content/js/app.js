$(document).ready(
    function () {
        sidebar.fill();
        checkdms();
        $('#sbutton').attr('disabled', true);
        $('#search').on('input change', function () {
            if ($(this).val().length != 0) {
                $('#sbutton').attr('disabled', false);
                if ($(this).val().length >= 3) {
                    searchresults();
                }
            }
            else
                $('#sbutton').attr('disabled', true);
        });
    },
);

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
    if (!event.target.matches('#search')) {
        $('#sresults').removeClass("active");
    }
}

$(document).bind('change', '.spages', function () {
    var value = $('.spages').val();
    sidebar.filter(value);
});

function toggle() {
    document.getElementById('settings-dropdown').classList.toggle("show");
}

function checkdms() {
    $.ajax({
        url: "/unreaddms",
        success: function (data) {
            if (data) {
                $('#dmEnvelope').toggleClass("active");
            }
        }
    });
}

function searchresults() {
    var search = { text: $('#search').val() };
    $.ajax({
        url: "/s/result",
        data: search,
        success: function (data) {
            if (data != null) {
                $('#sresults').empty().prepend(data);
                $('#sresults').addClass("active");
            }
        }
    });
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
    fillWall: function () {
        var username = $("#author").val();
        var data = { username: username };
        $.ajax({
            url: "/u/authorWall",
            data: data,
            success: function (data) {
                $('#wallEntries').empty().prepend(data);
            }
        });
    },
    fillFavourites: function () {
        var username = $("#author").val();
        var data = { username: username };
        $.ajax({
            url: "/u/authorFavourites",
            data: data,
            success: function (data) {
                $('#rss-entries').empty().prepend(data);
            }
        });
    },
    filter: function (page) {
        var name = $("#author").val();
        var filter = { page: page };
        $.ajax({
            url: "/u/authorEntries/" + name,
            data: filter,
            success: function (data) {
                $('#rss-entries').empty().prepend(data);
            }
        });
    },
    filterWall: function (page) {
        var name = $("#author").val();
        var filter = { page: page };
        $.ajax({
            url: "/u/authorWall/" + name,
            data: filter,
            success: function (data) {
                $('#wallEntries').empty().prepend(data);
            }
        });
    },
    filterFavourites: function (page) {
        var name = $("#author").val();
        var filter = { page: page };
        $.ajax({
            url: "/u/authorFavourites/" + name,
            data: filter,
            success: function (data) {
                $('#rss-entries').empty().prepend(data);
            }
        });
    },
}

var interactions = {
    setfollowstate: function (AuthorID) {
        var data = { UserID: AuthorID };
        $.ajax({
            url: "/i/setFollowState",
            data: data,
            success: function (data) {
                if (data == undefined || data == null) {
                    $('#followAuthor').toggleClass("active");
                    $('#followAuthor').text("follow");
                }
                else {
                    $('#followAuthor').toggleClass("active");
                    $('#followAuthor').text("following");
                }
            }
        });
    },
    setblockstate: function (AuthorID) {
        var data = { UserID: AuthorID };
        $.ajax({
            url: "/i/setBlockState",
            data: data,
            success: function (data) {
                if (data == undefined || data == null) {
                    $('#ignoreAuthor').toggleClass("active");
                    $('#ignoreAuthor').text("ignore");
                }
                else {
                    $('#ignoreAuthor').toggleClass("active");
                    $('#ignoreAuthor').text("ignored");
                }
            }
        });
    },
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
        var filter = { page: page };
        $.ajax({
            url: "/sideBar",
            data: filter,
            success: function (data) {
                localStorage.content = data;
                $('#s-content').empty().prepend(data);
                //return data;
            }
        });
    },
}

var entries = {
    archive: function (ID) {
        var model = { ID: ID, State: 0};
        if (confirm('go ahead with archiving this entry?')) {
            $.ajax({
                url: "/archive/entry",
                data: model,
                success: function (data) {
                    if (data) {
                        $("#def" + ID).closest("li").remove();
                    }
                }
            });
        }
    },
    archiveAuthor: function (ID) {
        var model = { ID: ID, State: 0 };
        if (confirm('go ahead with archiving this entry?')) {
            $.ajax({
                url: "/archive/entry",
                data: model,
                success: function (data) {
                    if (data) {
                        $("#def" + ID).closest(".rss-threads").remove();
                    }
                }
            });
        }
    },
    archiveView: function (ID) {
        var model = { ID: ID, State: 0 };
        if (confirm('go ahead with archiving this entry?')) {
            $.ajax({
                url: "/archive/entry",
                data: model,
                success: function (data) {
                    if (data) {
                        $(this).text("archived");
                    }
                }
            });
        }
    },
    archiveWall: function (ID) {
        var model = { ID: ID };
        if (confirm('go ahead with archiving this entry?')) {
            $.ajax({
                url: "/u/archive/wall",
                data: model,
                success: function (data) {
                    if (data) {
                        $("#def" + ID).remove();
                    }
                }
            });
        }
    },
    reinstate: function (ID) {
        var model = { ID: ID, State: 1 };
        if (confirm('reinstate this entry?')) {
            $.ajax({
                url: "/archive/entry",
                data: model,
                success: function (data) {
                    if (data) {
                        $("#def" + ID).closest(".rss-threads").remove();
                        if ($(".rss-threads").length < 1) {
                            $(".pagination").eq(1).remove();
                        }
                    }
                }
            });
        }
    },
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

        if (e.target.id == "buser") {
            if (sel < 1) {
                var val = textEditor.promt("which user did you mean?");
                if (val !== null) {
                    textEditor.appendUser(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapUser(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bentry") {
            if (sel < 1) {

                do {
                    var val = parseInt(textEditor.promt("enter entry ID, only numbers"), 10);
                } while (isNaN(val) || val < 1);

                if (val !== null) {
                    textEditor.appendEntry(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapEntry(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bthread") {
            if (sel < 1) {
                var val = textEditor.promt("which thread did you mean?");
                if (val !== null) {
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
                if (val !== null) {
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
                if (val !== null) {
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

    appendUser: function (textarea, len, start, end, val) {
        var replace = '[user ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapUser: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[user ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendEntry: function (textarea, len, start, end, val) {
        var replace = '[entry ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapEntry: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[entry ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendThread: function (textarea, len, start, end, val) {
        var replace = '[thread ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapThread: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[thread ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendLink: function (textarea, len, start, end, val) {
        var replace = '[link ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapLink: function (textarea, len, start, end, sel) {
        var replace = '[link ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendSpoiler: function (textarea, len, start, end, val) {
        var replace = '[spoiler ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapSpoiler: function (textarea, len, start, end, sel) {
        var replace = '[spoiler ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    }
}

var dmEditor = {
    checkSelection: function (e) {
        var textarea = document.getElementById("dmBody");
        var len = textarea.value.length;
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        var sel = textarea.value.substring(start, end);

        if (e.target.id == "buser") {
            if (sel < 1) {
                var val = textEditor.promt("which user did you mean?");
                if (val !== null) {
                    textEditor.appendUser(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapUser(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bentry") {
            if (sel < 1) {
                do {
                    var val = parseInt(textEditor.promt("enter entry ID, only numbers"), 10);
                } while (isNaN(val) || val < 1);

                if (val !== null) {
                    textEditor.appendEntry(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapEntry(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bthread") {
            if (sel < 1) {
                var val = textEditor.promt("which thread did you mean?");
                if (val !== null) {
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
                if (val !== null) {
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
                if (val !== null) {
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

    appendUser: function (textarea, len, start, end, val) {
        var replace = '[user ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapUser: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[user ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendEntry: function (textarea, len, start, end, val) {
        var replace = '[entry ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapEntry: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[entry ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendThread: function (textarea, len, start, end, val) {
        var replace = '[thread ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapThread: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[thread ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendLink: function (textarea, len, start, end, val) {
        var replace = '[link ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapLink: function (textarea, len, start, end, sel) {
        var replace = '[link ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendSpoiler: function (textarea, len, start, end, val) {
        var replace = '[spoiler ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapSpoiler: function (textarea, len, start, end, sel) {
        var replace = '[spoiler ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    }
}

var replyEditor = {
    replyWith: function (id, uid) {
        $("#dmBody").val("[entry " + id + "] ");
        $("#UserID").val(uid);
        $("#dm-modal").toggleClass("active");
    },

    checkSelection: function (e) {
        var textarea = document.getElementById("dmBody");
        var len = textarea.value.length;
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        var sel = textarea.value.substring(start, end);

        if (e.target.id == "buser") {
            if (sel < 1) {
                var val = textEditor.promt("which user did you mean?");
                if (val !== null) {
                    textEditor.appendUser(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapUser(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bentry") {
            if (sel < 1) {
                do {
                    var val = parseInt(textEditor.promt("enter entry ID, only numbers"), 10);
                } while (isNaN(val) || val < 1);

                if (val !== null) {
                    textEditor.appendEntry(textarea, len, start, end, val);
                }
            }
            else {
                textEditor.wrapEntry(textarea, len, start, end, sel);
            }
        }

        if (e.target.id == "bthread") {
            if (sel < 1) {
                var val = textEditor.promt("which thread did you mean?");
                if (val !== null) {
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
                if (val !== null) {
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
                if (val !== null) {
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

    appendUser: function (textarea, len, start, end, val) {
        var replace = '[user ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapUser: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[user ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendEntry: function (textarea, len, start, end, val) {
        var replace = '[entry ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapEntry: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[entry ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendThread: function (textarea, len, start, end, val) {
        var replace = '[thread ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapThread: function (textarea, len, start, end, sel) {
        //var replace = '<a href="' + sel + '">' + sel + '</a>';
        var replace = '[thread ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendLink: function (textarea, len, start, end, val) {
        var replace = '[link ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapLink: function (textarea, len, start, end, sel) {
        var replace = '[link ' + sel + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    appendSpoiler: function (textarea, len, start, end, val) {
        var replace = '[spoiler ' + val + '] ';
        textarea.value = textarea.value.substring(0, start) + replace + textarea.value.substring(end, len);
    },

    wrapSpoiler: function (textarea, len, start, end, sel) {
        var replace = '[spoiler ' + sel + '] ';
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
        Filter.refresh(window.location.pathname + "?keyword=" + querystring.get('keyword'));
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