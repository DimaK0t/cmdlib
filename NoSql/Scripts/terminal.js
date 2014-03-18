jQuery(document).ready(function ($) {

    $('#terminal').terminal(function (command, term) {
        this.url = "";
        
        if (command == 'login') {
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
        } else if (command == "help") {
            term.echo("books <page_number>");
            term.echo("updatedb/updatedb clean");
            term.echo("get <book_number>");
            term.echo("login");
            term.echo("logout");
        } else {
            term.echo("Type 'help' to get all avaible commands");
        }
    },
    {
        prompt: '>',
        greetings: "Welcome to the Library.",
    });
});

function login(term, user, password, callback) {
    $.post(url, { UserName: user, Password: password, RememberMe: false }).done(function(data) {
        if (data.length != 0) {
            callback(data);
            term.echo("Welcome, " + user);
        } else {
            callback(null);
        }
    }).fail(function(xhr) {
        term.error(xhr.statusText);;
    });
}

function logout(term) {
    $.post(url).done(function () { term.echo("You've successfully signed out"); }).fail(function(xhr) {
        term.error(xhr.statusText);
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
                            //"<div class='cell id'>" + book.Id + "</div>" +
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