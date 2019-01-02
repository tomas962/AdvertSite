// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function postComment(event) {
    event.preventDefault();

    let url = window.location.href;
    let vals = url.split("/");
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
                getComments(listingId);
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
                getComments(listingId);
            }
        })
        .catch(err => {
            console.log(err);
        })
}

function getComments(listingId) {
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
                response.text()
                    .then(htmlString => {
                        console.log(htmlString);

                        let commList = document.getElementById('commentList');
                        while (commList.firstChild) {
                            commList.removeChild(commList.firstChild);
                        }

                        $("#commentList").append(htmlString);
                    })
            }
        })
        .catch(err => {
            console.log(err);
        })
}