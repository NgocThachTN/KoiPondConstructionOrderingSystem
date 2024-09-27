import React from 'react';

function AdminPage({ isLoggedIn }) {
    return (
        <div>
            {!isLoggedIn && <button>Login</button>}
            {isLoggedIn && <p>Welcome to the admin page!</p>}
        </div>
    );
}

export default AdminPage;
