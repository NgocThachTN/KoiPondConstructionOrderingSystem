import React from 'react';
import Link from 'next/link';

const Navbar = ({ isLoggedIn, userName }) => {
    return (
        <nav>
            <ul>
                <li><Link href="/">Home</Link></li>
                <li><Link href="/about">About</Link></li>
                <li><Link href="/profile">Profile</Link></li>
                <li><Link href="/projects">Projects</Link></li>
                <li><Link href="/services">Services</Link></li>
                <li><Link href="/team">Team</Link></li>
                {/* Add other navigation links */}
                <li style={{ marginLeft: 'auto' }}>
                    {isLoggedIn ? <span>Welcome, {userName}</span> : <Link href="/login">Login</Link>}
                </li>
            </ul>
        </nav>
    );
};

export default Navbar;