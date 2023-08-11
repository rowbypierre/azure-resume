//FUNCTION CALL                                                                
window.addEventListener('DOMContentLoaded', (event) =>{         // When this content has loaded --> run even getVisitCount
    getVisitCount();
})

const functionApiUrl = 'https://getresumecounterrp.azurewebsites.net/api/GetResumeCounter?code=T4ua60NG3n2M7QCPy-GiJ4RiWOJw7jasHNHdlY2YPi1GAzFuvBtAKA=='
const localFunctionApi = 'http://localhost:7071/api/GetResumeCounter';

//CREATE FUNCTION
const getVisitCount = () => {
    fetch(functionApiUrl)
        .then(response => response.json()) // Parse response as JSON
        .then(data => {
            console.log("Website called function API.");
            const count = data.count; // Get the count value from the JSON data
            document.getElementById("counter").innerText = count;
        })
        .catch(error => {
            console.log(error);
        });
};
