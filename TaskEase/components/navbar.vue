<template>
  <nav class="custom-navbar">
    <ul class="nav-list">
      <li class="nav-item">
        <nuxt-link to="/usuarios" class="nav-link">Usuários</nuxt-link>
      </li>
      <li class="nav-item">
        <nuxt-link to="/" class="nav-link">Tarefas</nuxt-link>
      </li>
    </ul>
  </nav>
</template>

<style scoped>
.custom-navbar {
  background-color: #000;
  padding: 20px;
  font-family: Arial, Helvetica, sans-serif;
}

.nav-list {
  list-style-type: none;
  margin: 0;
  padding: 0;
  display: flex;
}

.nav-item {
  margin-right: 20px;
}

.nav-link {
  color: #fff;
  text-decoration: none;
  padding: 14px 16px;
}

.nav-link:hover {
  background-color: #333;
}
</style>
