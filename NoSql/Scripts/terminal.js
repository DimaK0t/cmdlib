﻿jQuery(document).ready(function ($) {

    $('#terminal').terminal(function (command, term) {
        this.url = "";
        command = command.toLowerCase();

        if (command == 'register') {
            url = "Account/Register";
            register(term);
        } else if (command == 'login') {
            term.login(function(user, password, callback) {
                url = "/Account/Login";
                login(term, user, password, callback);
            });
        } else if (command == 'logout') {
            url = "Account/Logoff";
            logout(term);
        } else if (command.indexOf('books') == 0) {
            url = "Home/GetBooks";
            var pageNumber = command.split(' ')[1];
            if (pageNumber == 'all') {
                pageNumber = -1;}
            getBooks(term, pageNumber);
        } else if (command.indexOf("updatedb") == 0) {
            var flag = command.split(' ')[1];
            url = "Home/UpdateDb";
            updateDb(term, flag);
        } else if (command.indexOf("get") == 0) {
            url = "Home/Validate";
            var number = command.split(' ')[1];
            getBook(term, number);
        } else if (command.toLowerCase() == "about") {
            url = "Home/About";
            about(term);
        } else if (command == "help") {
            term.echo("books <page_number>/books all");
            term.echo("get <book_number>");
            term.echo("about");
        } else {
            term.echo("Type 'help' to get all avaible commands");
        }
    },
    {
        prompt: '>',
        greetings: "Welcome to the Library.\n" +
                    "* Type 'help' to see all commands.",
    });
});

function about(term) {
    $.get(url).done(function(data) { term.echo(data, { raw: true }); }).fail(function(xhr) {
        term.error(xhr.statusText);
    });
}

function login(term, user, password, callback) {
    $.post(url, { UserName: user, Password: password, RememberMe: false }).done(function(data) {
        if (!data.IsFailed) {
            callback(data.Token);
            term.echo(data.Message);
        } else {
            callback(null);
        }
    }).fail(function(xhr) {
        term.error(xhr.statusText);
        callback(null);
    });
}

function logout(term) {
    $.post(url).done(function () { term.echo("You've successfully signed out"); }).fail(function(xhr) {
        term.error(xhr.statusText);
    });
}

function register(term) {
    term.push(function (userName) {
        term.set_mask(true).push(function (password) {
            term.push(function (comfirmPassword) {
                $.post(url, { UserName: userName, Password: password, ConfirmPassword: comfirmPassword })
                    .done(function (data) {
                        if (!data.IsFailed) {
                            term.echo(data.Message);
                        } else {
                            $.each(data.ErrorMessages, function (i, error) {
                                term.error(error);
                            });
                        }
                    }).
                fail(function(xhr) {
                    term.error(xhr.statusText);
                });

                term.pop().pop().pop();
            }, {
                prompt: 'confirm password:'
            }
            );
        }, {
            prompt: 'password:'
        });
    }, {
        prompt: 'user name:'
    });
}

function getBook(term, number) {
    $.get(url, { number: number })
        .done(function() {
            window.location = window.location + "Home/GetBook" + '/' + number; 
            term.echo("done");
        })
        .fail(function(xhr) {
            term.error(xhr.statusText);
            term.error("enter correct book number");
        });
}

function updateDb(term, flag) {
    $.post(url, { flag: flag })
        .done(function () { term.echo("DB has been updated"); })
        .fail(function(xhr) {
            term.error(xhr.statusText);
    });
}

function getBooks(term, pageNumber) {
    $.post(url, { pageNumber: pageNumber })
        .done(
            function(data) {
                var str = "<div class='table'>";
                $.each(data.Books, function(i, book) {
                    str = str +
                        "<div class='row'>" +
                            "<div class='cell'>" + book.BookNumber + "</div>" +
                            "<div class='cell'>" + book.Author + "</div>" +
                            "<div class='cell'>" + book.Name + "</div>" +
                            "<div class='cell'>" + book.Extension + "</div>" +
                        "</div>";
                });
                str = str + "</div>";
                term.echo("Page " + data.CurrentPage + " of " + data.PagesCount);
                term.echo(str, { raw: true });
            })
        .fail(function(xhr) {
            term.error(xhr.statusText);;
        });
}