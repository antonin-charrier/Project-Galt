class AuthService {
    login() {
        var popup = window.open('/Account/ExternalLogin?provider=GitHub',
            "Connexion à Galt", "menubar=no, status=no, scrollbars=no, menubar=no, width=700, height=700");
    }
}

export default new AuthService();