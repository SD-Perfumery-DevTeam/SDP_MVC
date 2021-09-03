var content = "";

const productCon = document.querySelectorAll(".product_con");
const row = document.querySelector("#rowone");



productDbJson.forEach(displayCardFunc);



function displayCardFunc(prod) {
    let type;
    switch (prod.productType) {
        case 0:
            type = "EDP"
            break;
        case 1:
             type = "EDT"
            break;
    }
    
    content =
        ` 
        <div class="card" style="width: 18rem;">
            <img src="../imgs/${prod.imgUrl}" class="card-img-top" alt="...">
            <div class="card-body d-flex flex-column d-flex">
                <h5>${prod.title} by ${prod.brand} ${type} ${prod.packageQty}ml </h5>
                <h5>$${prod.price}</h5>
                <h5></h5>
                <h5 class="card-title">
                some description
                </h5>

                <button class="btn btn-primary mt-auto card-btn"  type="submit" name= "value"  value = "${prod.productId}"  >View Detail</button>
            </div>
        </div>
    `;
    let node = document.createElement("div");
    node.setAttribute("class", "col-sm  d-flex align-items-stretch");
    row.appendChild(node);
    node.innerHTML = content;

}


function comparePrice(a, b) {

    if (a.price < b.price) {
        return -1;
    }
    if (a.price > b.price) {
        return 1;
    }
    return 0;
}

function comparePrice2(a, b) {

    if (a.price > b.price) {
        return -1;
    }
    if (a.price < b.price) {
        return 1;
    }
    return 0;
}

function compareName(a, b) {
    if (a.name < b.name) {
        return -1;
    }
    if (a.name > b.name) {
        return 1;
    }
    return 0;
}
function compareName2(a, b) {
    if (a.name > b.name) {
        return -1;
    }
    if (a.name < b.name) {
        return 1;
    }
    return 0;
}

function sortByPriceHL() {
    productDbJson.sort(comparePrice);
    row.innerHTML = "";
    productDbJson.forEach(displayCardFunc);
}
function sortByPriceLH() {
    productDbJson.sort(comparePrice2);
    row.innerHTML = "";
    productDbJson.forEach(displayCardFunc);
}
function sortByNameAZ() {
    productDbJson.sort(compareName);
    row.innerHTML = "";
    productDbJson.forEach(displayCardFunc);
}
function sortByNameZA() {
    productDbJson.sort(compareName2);
    row.innerHTML = "";
    productDbJson.forEach(displayCardFunc);
}


function sortBy(byWat) {
    var text = byWat.options[byWat.selectedIndex].value;
    if (text === "priceH-L") {
        sortByPriceHL();
        console.log("changed");
    }
    else if (text === "priceL-H") {
        sortByPriceLH();
    }
    else if (text === "nameA-Z") {
        sortByNameAZ();
    }
    else if (text === "nameZ-A") {
        sortByNameZA();
    }
}