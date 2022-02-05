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

var entryAttribute = {
    voteUP: function (EntryID) {
        var model = { EntryID: EntryID, Vote: 1};
        $.ajax({
            url: "/vote",
            data: model,
            success: function (model) {
                if (model == null || model.vote == 0) {
                    $("#def" + EntryID).find(".downvoted").removeClass("active");
                    $("#def" + EntryID).find(".upvoted").removeClass("active");
                }
                else if (model.vote == 1) {
                    $("#def" + EntryID).find(".upvoted").toggleClass("active");
                    $("#def" + EntryID).find(".downvoted").removeClass("active");
                }
            }
        });
    },
    voteDOWN: function (EntryID) {
        var model = { EntryID: EntryID, Vote: 2};
        $.ajax({
            url: "/vote",
            data: model,
            success: function (model) {
                if (model == null || model.vote == 0) {
                    $("#def" + EntryID).find(".downvoted").removeClass("active");
                    $("#def" + EntryID).find(".upvoted").removeClass("active");
                }
                else if (model.vote == 2) {
                    $("#def" + EntryID).find(".downvoted").toggleClass("active");
                    $("#def" + EntryID).find(".upvoted").removeClass("active");
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
                    $("#def" + model.entryID).find(".fav").toggleClass("active");
                }
                else if (!model.favourite) {
                    $("#def" + model.entryID).find(".fav").removeClass("active");
                }
                else if (model.favourite == null) {
                    $("#def" + model.entryID).find(".fav").removeClass("active");
                }
            }
        });
    }
}

const bthread = document.getElementById("bthread");
const bspoiler = document.getElementById("bspoiler");
const blink = document.getElementById("blink");
bthread.addEventListener("click", this.checkSelection);
bspoiler.addEventListener("click", this.checkSelection);
blink.addEventListener("click", this.checkSelection);

function checkSelection(e) {
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
}

var textEditor = {
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