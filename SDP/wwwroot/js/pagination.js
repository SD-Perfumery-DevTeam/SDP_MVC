var current_page = 1;
var productPerPage = 16;
var displayList;

function prevPage() {
    if (current_page > 1) {
        current_page--;
        changePage(current_page);
    }
}

function nextPage() {
    if (current_page < numPages()) {
        current_page++;
        changePage(current_page);
    }
}

function changePage(page) {




    // Validate page
    if (page < 1) page = 1;
    if (page > numPages()) page = numPages();



    for (var i = (page - 1) * productPerPage; i < (page * productPerPage); i++) {

        if (i <= productDbJson.length-1) { displayList.push(productDbJson[i]); }

    }
    console.log(productDbJson);
    console.log(displayList);
    displayList.forEach(displayCardFunc); 
    productDbJson.forEach(displayCardFunc);

}

function numPages() {
    return Math.ceil(productDbJson / productPerPage);
}

window.onload = function () {
    changePage(1);
}