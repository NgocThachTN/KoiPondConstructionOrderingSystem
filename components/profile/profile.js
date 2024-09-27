import React from 'react';

function ProfileComponent({ isLoggedIn }) {
    return (
        <div>
            {!isLoggedIn && <button>Login</button>}
            {isLoggedIn && <p>Profile content...</p>}
        </div>
    );
}

export default ProfileComponent;
