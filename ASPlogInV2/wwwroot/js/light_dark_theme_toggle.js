document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById("dark-toggle");
    const toggleBtnIcon = toggleBtn.querySelector(".toggler-icon");

    //Setting the user theme setting based on local storage from browser
    const savedTheme = localStorage.getItem('theme');
    const systemIsDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

    let isDarkMode = savedTheme ? savedTheme == 'dark' : systemIsDark;

    function applyTheme(darkMode) {
        //Removes the light and dark modes so it doesn't accidentally add both at same time or two of the same.
        document.documentElement.classList.remove('light-mode', 'dark-mode');

        //If darkMode is selected
        if (darkMode) {
            document.documentElement.classList.add('dark-mode');
            toggleBtnIcon.textContent = '🌙';
            toggleBtn.setAttribute('aria-label', 'Switch to light mode');
        }
        //If lightMode is selected
        else {
            document.documentElement.classList.add('light-mode');
            toggleBtnIcon.textContent = '🔆';
            toggleBtn.setAttribute('aria-label', 'switch to dark mode');
        }
        //Sets local storage to the updated user prefered theme type
        isDarkMode = darkMode;
        localStorage.setItem('theme', darkMode ? 'dark' : 'light');
    }

    //Button function when button is clicked
    function changeTheme() {
        applyTheme(!isDarkMode);
    }

    //Sets the theme when program starts
    applyTheme(isDarkMode);

    //Event listener, allows code to actually detect if user clicked toggle or not
    toggleBtn.addEventListener('click', changeTheme);
});