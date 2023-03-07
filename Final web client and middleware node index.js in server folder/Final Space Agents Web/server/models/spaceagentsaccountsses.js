module.exports = (sequelize, DataTypes) => {
    const spaceagentsaccountsses = sequelize.define("spaceagentsaccountsses", {
      username: {
        type: DataTypes.STRING,
        allowNull: false,
        unique: true,
      },
      password: {
        type: DataTypes.STRING,
        allowNull: false,
      },
    });
    return spaceagentsaccountsses;
  };