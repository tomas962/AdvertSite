// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (window.location.href.includes("Listings/Details")) {
    let vals = window.location.href.split("/");
    let listingId = vals[vals.length - 1];
    getComments(listingId).then((comments) => { $.ready.then(() => { appendComments(comments) }) });
}


function postComment(event) {
    event.preventDefault();

    let vals = window.location.href.split("/");
    let listingId = vals[vals.length - 1];
    fetch('/Comment/CreateAjax?' + $.param({
        Id: listingId,
        "Comment.Text": document.getElementById('Comment.Text').value
        //__RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value
    }), {
            credentials: 'include',
            method: 'POST'
        })
        .then((response) => {
            if (response.status == 200) {
                getComments(listingId).then((comments) => { $.ready.then(() => { appendComments(comments) }) });
            }
        })
        .catch(err => {
            console.log(err);
        })
    return false;
}

function deleteComment(event) {
    let url = window.location.href;
    let vals = url.split("/");
    let listingId = vals[vals.length - 1];
    fetch('/Comment/Delete/' + event.target.value,
        {
            credentials: "include",
            method: "POST"
        })
        .then(response => {
            if (response.status == 200) {
                getComments(listingId).then((comments) => { $.ready.then(() => { appendComments(comments) }) });
            }
        })
        .catch(err => {
            console.log(err);
        })
}

function getComments(listingId) {
    return new Promise((resolve, reject) => {
        fetch('/Comment/GetComments?' + $.param({
            listingId: listingId
            //__RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value
        }), {
                credentials: 'include',
                method: 'GET'
            }
        )
            .then(response => {
                if (response.status == 200) {
                    response.json()
                        .then(comments => {
                            resolve(comments);
                        })
                }
            })
            .catch(err => {
                console.log(err);
            })
    });
    
}

function appendComments(comments) {
    let htmlString = "";
    comments.forEach((value) => {
        if (value.canDelete)
            htmlString += `<dt>${value.userName}</dt><dt>${value.date.split("T")[0] + " " + value.date.split("T")[1].substring(0, 8)}</dt><dd>${value.text} <button class='btn btn-danger' onclick='deleteComment(event)' value='${value.id}'>Šalinti</button></dd>`;
        else
            htmlString += `<dt>${value.userName}</dt><dt>${value.date.split("T")[0] + " " + value.date.split("T")[1].substring(0, 8)}</dt><dd>${value.text} </dd>`;
    })


    let commList = document.getElementById('commentList');
    while (commList.firstChild) {
        commList.removeChild(commList.firstChild);
    }
    $("#comment-count").empty().append(comments.length);
    $("#commentList").append(htmlString);
}

$("#nav-listings").click((e) => {
    localStorage.removeItem("category");
    localStorage.removeItem("subcategory");
});