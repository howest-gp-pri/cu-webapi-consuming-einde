window.addEventListener("load", initialize);

const baseUrl = "https://localhost:7011/api/";
let categories = [];

function initialize() {
    const btnLogin = document.querySelector("#login");
    btnLogin.addEventListener("click", login);

    fetch(`${baseUrl}categories`)
        .then((response) => response.json())
        .then((data) => {
            categories = data;
            showCategories();
        })
        .catch(error => console.log(error));
}

function showCategories() {
    const divContainer = document.querySelector(".container");
    categories.forEach(category => {
        const div = document.createElement("div");
        div.classList.add("category");
        div.innerHTML = `<h2>${category.name}</h2>`;
        divContainer.appendChild(div);
    });
}

function login() {
    const email = document.querySelector("#email").value;
    const password = document.querySelector("#password").value;
    const body = {
        email: email,
        password: password
    };

    fetch(`${baseUrl}auth/login`, {
        method: "POST",
        body: JSON.stringify(body),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            const divLogin = document.querySelector(".login");
            divLogin.classList.add("hidden");

            const divCreateCategory = document.querySelector(".btn-create");
            divCreateCategory.addEventListener("click", showCategoryForm);

            divCreateCategory.classList.remove("hidden");

            sessionStorage.setItem("token", data.token);
        })
        .catch(error => console.log(error));
}

function showCategoryForm() {
    const divForm = document.querySelector(".new-category");
    divForm.classList.remove("hidden");
    const btnCreate = document.querySelector("#create-category");
    btnCreate.addEventListener("click", postCategory);
}

function postCategory() {
    const name = document.querySelector("#category").value;
    const body = {
        name: name
    };

    fetch(`${baseUrl}categories`, {
        method: "POST",
        body: JSON.stringify(body),
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${sessionStorage.getItem("token")}`
        }
    })
        .then(response => response.json())
        .then(data => {
            const divForm = document.querySelector(".new-category");
            divForm.classList.add("hidden");

            const divContainer = document.querySelector(".container");
            const div = document.createElement("div");
            div.classList.add("category");
            div.innerHTML = `<h2>${name}</h2>`;
            divContainer.appendChild(div);
        })
        .catch(error => console.log(error));
}