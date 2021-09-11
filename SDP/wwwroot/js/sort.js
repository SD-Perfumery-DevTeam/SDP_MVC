var content = "";

const productCon = document.querySelectorAll(".product_con");
const row = document.querySelector("#product-list");



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
        <div>
            <img src="../imgs/${prod.imgUrl}" alt="${prod.title}">
            <div class="product-summary">
                <h4>${prod.title} (X)</h4><!-- Need to include (M) or (W) for gender here. -->
                <p>by ${prod.brand}</p>
                <p><small>${type}</small></p>
                <p><small>${prod.packageQty}ml</small></p><!-- Needs dynamic UoM here -->
            </div>
            <div>
                <h5>Rs. ${prod.price}</h5>
                <button type="submit" name= "value"  value = "${prod.productId}"  >View Detail</button></div>
            </div>
    `;
    let node = document.createElement("div");
    node.setAttribute("class", "product");
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

// TODO - The logic for price sort seems to be switched H-L and L-H.
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